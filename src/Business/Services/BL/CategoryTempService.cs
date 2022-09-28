using Common.ViewModels.BaseModel;
using Common.ViewModels.CategoryTemp;
using Repository.Entities;
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

        public BaseResponseModel<string> DeleteTemp(CategoryTemp categoryTemp)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var temp = _unitOfWork._db.CategoryTemp.FirstOrDefault(x => x.Id == categoryTemp.Id);
                if(temp == null)
                {
                    response.Status = "404";
                    response.Message = "Object not found";
                    return response;
                }
                else
                {
                    _unitOfWork._db.CategoryTemp.Remove(temp);
                    _unitOfWork._db.SaveChanges();
                }
                response.Status = "00";
                response.Message = "Object deleted successfully";
                return response;
                
            }catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Exception occurred :" + ex.Message;
                return response;
            }
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
                    temp.Id = categoryTemp.Id;
                    temp.CategoryName = categoryTemp.CategoryName;

                    _unitOfWork._db.CategoryTemp.Update(temp);
                    _unitOfWork._db.SaveChanges();
                }
                response.Status = "00";
                response.Message = "Category Edited sucessfully";
                return response;
            }
            catch(Exception ex)
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
        BaseResponseModel<string> DeleteTemp(CategoryTemp categoryTemp);
    }
}
