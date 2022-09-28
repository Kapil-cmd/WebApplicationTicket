using Common.ViewModels.BaseModel;
using Common.ViewModels.CategoryTemp;
using Repository.Repos.Work;

namespace Services.BL
{
    public class CategoryTempService : ICategoryTempService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryTempService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseResponseModel<string> EditTemp(EditCategoryTemp categoryTemp)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var temp = _unitOfWork._db.CategoryTemp.FirstOrDefault(x => x.Id == categoryTemp.Id);
                if(temp == null)
                {
                    response.Status = "404";
                    response.Message = "Category Not Found";
                    return response;
                }
                else
                {
                    _unitOfWork._db.CategoryTemp.Update(temp);
                    _unitOfWork._db.SaveChanges();
                }
                response.Status = "00";
                response.Message = "Category Edited sucessfully";
                return response;
            }catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Exception occurred :" + ex.Message;
                return response;
            }
        }
    }
    public interface ICategoryTempService
    {
        BaseResponseModel<string> EditTemp(EditCategoryTemp categoryTemp );
    }
}
