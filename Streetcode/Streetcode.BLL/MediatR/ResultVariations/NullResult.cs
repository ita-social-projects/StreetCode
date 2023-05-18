using FluentResults;

namespace Streetcode.BLL.MediatR.ResultVariations
{
    public class NullResult<T> : Result<T>
    {
        public NullResult()
            : base()
        {
        }
    }
}
