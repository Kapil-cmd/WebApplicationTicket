using Common.ViewModels.BaseModel;
using Common.ViewModels.Users;
using Repository.Entities;
using Repository.Repos.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BL
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork unitOfWork;
        public BaseResponseModel<UserViewModel> Login(UserLogin model)
        {
            var response = new BaseResponseModel<UserViewModel>();

            try
            {
                UserViewModel data = new UserViewModel();
                data.Id = "1";

                response.Status = "200";
                response.Message = "Successfully logged in.";
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Status = "500";
                response.Message = "Exception occured: " + ex.Message;
                response.Data = null;
            }
            return response;   
        }
        public BaseResponseModel<UserViewModel> Register(UserRegister Register)
        {
            var response = new BaseResponseModel<UserViewModel>();
            try
            {
                UserViewModel model = new UserViewModel();
            }
        }
        public BaseResponseModel<UserViewModel> EditUser(EditUserViewModel EditUser)
        {

        }
    }

    public interface IUserService
    {
        BaseResponseModel<UserViewModel> Login(UserLogin Login);
        BaseResponseModel<UserViewModel> Register(UserRegister Register);
        BaseResponseModel<UserViewModel> EditUser(EditUserViewModel Edituser);
        
    }
}
