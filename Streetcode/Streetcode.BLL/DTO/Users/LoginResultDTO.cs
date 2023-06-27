using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Users
{
    public class LoginResultDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
