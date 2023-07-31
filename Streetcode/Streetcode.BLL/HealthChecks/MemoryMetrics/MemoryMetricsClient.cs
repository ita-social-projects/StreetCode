﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.Logging;
using Streetcode.DAL.Entities.Feedback;

namespace Streetcode.BLL.HealthChecks.MemoryMetrics
{
    public class MemoryMetricsClient
    {
        private readonly ILoggerService _loggerService;
        public MemoryMetricsClient(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public MemoryMetrics GetMetrics()
        {
            MemoryMetrics metrics;

            var watch = new Stopwatch();
            watch.Start();
            if (IsUnix())
            {
                metrics = GetUnixMetrics();
            }
            else
            {
                metrics = GetWindowsMetrics();
            }

            watch.Stop();
            metrics.Duration = watch.ElapsedMilliseconds;

            return metrics;
        }

        private bool IsUnix() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                                  RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private MemoryMetrics GetWindowsMetrics()
        {
            var output = string.Empty;

            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split($"{Environment.NewLine}");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }

        private MemoryMetrics GetUnixMetrics()
        {
            var output = "";
            string separator_new_line = Environment.NewLine;
            const string SEPARATOR_SPACE = " ";

            var info = new ProcessStartInfo("free -m");
            info.FileName = "/bin/bash";
            info.Arguments = "-c \"free -m\"";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
                _loggerService.LogInformation(output);
            }

            var lines = output.Split(separator_new_line);
            var memory = lines[1].Split(SEPARATOR_SPACE, StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);

            return metrics;
        }
    }
}