using Common.ViewModels.BaseModel;
using Common.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;
using Repository.Entites;
using Repository.Repos.Work;
using System.Security.Claims;

namespace Services.BL
{
    public class CategoryServices:ICategoryservice
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public CategoryServices(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public  BaseResponseModel<string> AddCategory(AddCategoryViewModel category)
        {
          var response = new BaseResponseModel<string>();
            try
            {
                if (_unitOfWork.CategoryRepository.Any(x => x.CategoryName == category.CategoryName))
                {
                    response.Status = "97";
                    response.Message = "Category with same name already exists";
                    return response;
                }

                var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                _unitOfWork._db.Category.Add(new Repository.Entites.Category()
                {
                    CategoryName = category.CategoryName,
                    CreatedBy = category.CreatedBy = nameClaim,
                    CreatedDateTime = DateTime.UtcNow,
                });

                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Category Added Successfully";
                return response;
            }
            catch(Exception ex)
            {
                response.Status = "500";
                response.Message ="Error occured" + ex.Message;
                return response;
            }
        }

        public BaseResponseModel<string> DeleteCategory(Category category)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var model = _unitOfWork._db.Category.FirstOrDefault(x => x.CId == category.CId);
                if(model == null)
                {
                    response.Status = "404";
                    response.Message = "Category not found";
                    return response;
                }
                var categoryTickets = _unitOfWork._db.Tickets.Any(x => x.CategoryName == category.CategoryName);
                if(categoryTickets == null)
                {
                    response.Status = "404";
                    response.Message = "Categpry not found";
                    return response;
                }
                else
                {
                    _unitOfWork._db.Remove(categoryTickets);
                    _unitOfWork._db.SaveChanges();
                }
                {
                    model = category;
                    _unitOfWork._db.Category.Remove(model);
                    _unitOfWork._db.SaveChanges();
                }

                response.Status = "00";
                response.Message = "Category deleted sucessfully";
                return response;
            }catch(Exception ex)
            {
                response.Status = "100";
                response.Message ="Exception occurred :"+ ex.Message;
                return response;
            }
        }

        public BaseResponseModel<string> EditCategory(EditCategoryViewModel EditCategory)
        {
            var response = new BaseResponseModel<string>();
            try
            {
                var category = _unitOfWork._db.Category.FirstOrDefault(x => x.CId == EditCategory.CId);
                if(category == null)
                {
                    response.Status = "100";
                    response.Message = "Category not Found";
                    return response;
                }
                var nameClaim = _unitOfWork._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                category.CId = EditCategory.CId;
                category.CategoryName = EditCategory.CategoryName;
                category.ModifiedDateTime = DateTime.UtcNow;
                category.ModifiedBy = EditCategory.ModifiedBy = nameClaim;

               

                _unitOfWork._db.Category.Update(category);
                _unitOfWork._db.SaveChanges();

                response.Status = "00";
                response.Message = "Category Updated Sucessfully";
                return response;
            }
            catch(Exception ex)
            {
                response.Status = "500";
                response.Message = "Error occured: " + ex.Message;
                return response;
            }
        }
       
    }
    public interface ICategoryservice
    {
        BaseResponseModel<string> AddCategory(AddCategoryViewModel model);
        BaseResponseModel<string> EditCategory(EditCategoryViewModel EditCategory);
        BaseResponseModel<string> DeleteCategory(Category category);
        
    }
}
