using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Users.SignUp
{
    public class SignUpHandler : IRequestHandler<SignUpQuery, Result<UserDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public SignUpHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public Task<Result<UserDTO>> Handle(SignUpQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
