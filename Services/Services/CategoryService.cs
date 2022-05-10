using Common.Exceptions;
using Common.Utilities;
using Data;
using Entities.DTO;
using Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Services.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Image = System.Drawing.Image;
using System.Drawing.Imaging;

namespace Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMemoryCache _treeCache;
        private readonly ApplicationDbContext _dbContext;

        public CategoryService(
            IMemoryCache treeCache,
            ApplicationDbContext dbContext)
        {
            _treeCache = treeCache;
            _dbContext = dbContext;
        }

        public async Task<List<int>> FlattenedTree(int CategoryId)
        {
            List<int> res = new();

            res.Add(CategoryId);

            var childs = await _dbContext.Categories
                .Where(x => x.ParentCategoryId.Value == CategoryId)
                .Select(s => s.Id)
                .ToListAsync();

            res.AddRange(childs);

            foreach (var item in childs)
            {
                res.AddRange(await FlattenedTree(item));
            }

            _treeCache.Set(CategoryId, res);

            return res;
        }

        public async Task<List<int>> GetRootNodesIds()
        {
            return await _dbContext.Categories
                  .Where(x => x.ParentCategoryId == null)
                  .Select(s => s.Id)
                  .ToListAsync();
        }

        public async Task FirstRunAfterBoot()
        {
            var ls = await GetRootNodesIds();
            foreach (var item in ls)
            {
                await FlattenedTree(item);
            }
        }

        public List<int> GetTreeFromCache(int CategoryId)
        {
            return _treeCache.Get<List<int>>(CategoryId);
        }

        public async Task<List<CategoryToReturnDTO>> RootCategories()
        {
            return await _dbContext.Categories
                .Where(x => x.ParentCategoryId == null)
                .OrderBy(o => o.Arrange)
                .Select(x => new CategoryToReturnDTO
                {
                    ImageUrl_L = x.ImageUrl_L,
                    ImageUrl_M = x.ImageUrl_M,
                    ImageUrl_S = x.ImageUrl_S,
                    Level = x.Level,
                    Name = x.Name,
                    ArrangeId = x.Arrange,
                    CategoryId = x.Id
                }).ToListAsync();
        }

        public async Task<List<CategoryToReturnDTO>> BrandsOfCategory(int rootId)
        {
            return await _dbContext.Categories
                .Where(x => x.ParentCategoryId == rootId)
                .OrderBy(o => o.Arrange)
                .Select(x => new CategoryToReturnDTO
                {
                    ImageUrl_L = x.ImageUrl_L,
                    ImageUrl_M = x.ImageUrl_M,
                    ImageUrl_S = x.ImageUrl_S,
                    Level = x.Level,
                    Name = x.Name,
                    ArrangeId = x.Arrange,
                    CategoryId = x.Id
                })
                .ToListAsync();
        }

        public async Task<List<CategoryToReturnDTO>> AllBrands()
        {
            var categoryResult = await _dbContext.Categories
                .Where(x => x.ParentCategory != null && x.ParentCategory.ParentCategoryId == null)
                .OrderBy(o => o.Arrange)
                //.Where(x => x.Level == 2)
                .Select(x => new CategoryToReturnDTO
                {
                    ImageUrl_L = x.ImageUrl_L,
                    ImageUrl_M = x.ImageUrl_M,
                    ImageUrl_S = x.ImageUrl_S,
                    Level = x.Level,
                    Name = x.Name,
                    ArrangeId = x.Arrange,
                    CategoryId = x.Id
                })
                .ToListAsync();

            return categoryResult;
        }

        public async Task<List<CategoryToReturnDTO>> ModelsOfBrand(int brandCategoryid)
        {
            return await _dbContext.Categories
                //.Where(x => x.Level == 3)
                //.Where(x => x.ChildCategories.Count == 0 && x.ParentCategoryId==brandCategoryid)
                .Where(x => x.ParentCategoryId == brandCategoryid)
                .OrderBy(o => o.Arrange)
                .Select(x => new CategoryToReturnDTO
                {
                    ImageUrl_L = x.ImageUrl_L,
                    ImageUrl_M = x.ImageUrl_M,
                    ImageUrl_S = x.ImageUrl_S,
                    Level = x.Level,
                    Name = x.Name,
                    ArrangeId = x.Arrange,
                    CategoryId = x.Id
                })
                .ToListAsync();
        }

        public async Task<List<int>> PathToRoot(int categoryId)
        {
            List<int> path = new();

            var parentId = await _dbContext.Categories
                .Where(x => x.Id == categoryId)
                .Select(x => x.ParentCategoryId)
                .FirstOrDefaultAsync();

            path.Add(categoryId);

            if (parentId is null)
            {
                return path;
            }

            path.AddRange(await PathToRoot(parentId.Value));

            return path;
        }

        public async Task<List<int>> MyPathToRoot(List<int> path, int categoryId)
        {
            var res = await _dbContext.Categories
                .Where(x => x.Id == categoryId)
                .FirstOrDefaultAsync();

            if (res != null)
            {
                path.Add(res.Id);

                if (res.ParentCategoryId != null)
                {
                    //var parentCat = _dbContext.Categories.Where(x => x.Id == res.ParentCategoryId)
                    //    .FirstOrDefault();

                    //foreach (var item in res.ChildCategories)
                    //{
                    await MyPathToRoot(path, res.ParentCategoryId.Value);
                    //}
                }
            }
            return path;
        }

        public async Task<List<int>> PathToLeaf(int rootId)
        {
            List<int> path = await _dbContext.Categories
                .Where(x => x.ParentCategoryId == rootId)
                .Select(s => s.Id)
                .ToListAsync();

            if (path.Count == 0)
            {
                return path;
            }
            else
            {
                path.ForEach(async x =>
                {
                    path.AddRange(await PathToLeaf(x));
                });
            }

            path.Add(rootId);

            return path;
        }

        public async Task<List<int>> MyPathToLeaf(List<int> path, int rootId)
        {
            var res = await _dbContext.Categories
                .Include(i => i.ChildCategories)
                .Where(x => x.Id == rootId)
                .FirstOrDefaultAsync();

            if (res != null)
            {
                path.Add(res.Id);
                if (res.ChildCategories != null)
                {
                    foreach (var item in res.ChildCategories)
                    {
                        await MyPathToLeaf(path, item.Id);
                    }
                }
            }
            return path;
        }

        public async Task<bool> AddCategory(CategoryForSetDTO dto)
        {
            if (dto.ParentCategoryId != null && !(await _dbContext.Categories.AnyAsync(x => x.Id == dto.ParentCategoryId)))
                throw new BadRequestException($"there is no category with : {dto.ParentCategoryId}");

            var latestCategoryId =
                await _dbContext.Categories
                .OrderByDescending(o => o.Id)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            var latestArrangeId =
                await _dbContext.Categories
                .Where(x => x.ParentCategoryId == dto.ParentCategoryId)
                .OrderByDescending(o => o.Arrange)
                .Select(s => s.Arrange)
                .FirstOrDefaultAsync();

            var categoryId = 1;

            categoryId += latestCategoryId;

            var category = new Category
            {
                Name = dto.Name,
                ParentCategoryId = dto.ParentCategoryId,
                Arrange = latestArrangeId + 1
            };

            var fileNameBase = categoryId.ToString();


            var path = @"wwwroot/Categories/";

            var source = dto.Img.OpenReadStream();

            var image = Image.FromStream(source);

            var fileNameS = fileNameBase + "_S.jpeg";
            var fileNameM = fileNameBase + "_M.jpeg";
            var fileNameL = fileNameBase + "_L.jpeg";

            ImageHelper.SaveJpeg(source, 200, 200, path + fileNameS, 70);
            ImageHelper.SaveJpeg(source, 400, 400, path + fileNameM, 70);
            ImageHelper.SaveJpeg(source, image.Height, image.Width, path + fileNameL, 70);

            //image.Save(path + fileNameL, ImageFormat.Png);


            category.ImageUrl_S = "/Categories/" + fileNameS;
            category.ImageUrl_M = "/Categories/" + fileNameM;
            category.ImageUrl_L = "/Categories/" + fileNameL;

            _dbContext.Categories.Add(category);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var existing = await _dbContext.Categories
                .Include(i => i.ChildCategories)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null)
            {
                throw new BadRequestException("category not exist");
            }
            if (existing.ChildCategories.Any())
            {
                throw new PolicyException("category has child");
            }
            _dbContext.Categories.Remove(existing);

            var path = @"wwwroot";
            ImageHelper.RemoveJpeg(path + existing.ImageUrl_L);
            ImageHelper.RemoveJpeg(path + existing.ImageUrl_M);
            ImageHelper.RemoveJpeg(path + existing.ImageUrl_S);


            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCategory(int catId, CategoryForSetDTO category)
        {
            var existing = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == catId);

            if (existing == null)
                return false;

            var fileNameBase = existing.Id.ToString();

            var fileNameS = fileNameBase + "_S.jpeg";
            var fileNameM = fileNameBase + "_M.jpeg";
            var fileNameL = fileNameBase + "_L.jpeg";

            var path = @"wwwroot/Photos/";

            var source = category.Img.OpenReadStream();

            ImageHelper.SaveJpeg(source, 200, 200, path + fileNameS, 70);
            ImageHelper.SaveJpeg(source, 600, 600, path + fileNameM, 70);
            ImageHelper.SaveJpeg(source, 1200, 1200, path + fileNameL, 70);

            existing.ImageUrl_L = "/Categories/" + fileNameL;
            existing.ImageUrl_M = "/Categories/" + fileNameM;
            existing.ImageUrl_S = "/Categories/" + fileNameS;
            existing.Name = category.Name;
            existing.ParentCategoryId = category.ParentCategoryId;

            _dbContext.Categories.Update(existing);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<CategoryToReturnDTO>> AllModels()
        {
            var categoryResult = await _dbContext.Categories
                .Include(i => i.ParentCategory)
                .ThenInclude(i => i.ParentCategory)
                .Where(x => x.ParentCategory != null &&
                        x.ParentCategory.ParentCategory != null &&
                        x.ParentCategory.ParentCategory.ParentCategoryId == null)
                //.Where(x => x.Level == 3)
                .Select(x => new CategoryToReturnDTO
                {
                    ImageUrl_L = x.ImageUrl_L,
                    ImageUrl_M = x.ImageUrl_M,
                    ImageUrl_S = x.ImageUrl_S,
                    Level = x.Level,
                    Name = x.Name,
                    ArrangeId = x.Arrange,
                    CategoryId = x.Id
                })
                .ToListAsync();

            return categoryResult;
        }

        public async Task<List<CategoryToReturnDTO>> CategoryArrange(int? parentId, List<int> arrangeIds)
        {
            var categories = await _dbContext.Categories
                 .Where(x => x.ParentCategoryId == parentId)
                 .ToListAsync();

            if (categories.Count != arrangeIds.Count ||
                categories.Select(s => s.Arrange).Any(x => !arrangeIds.Contains(x)))
            {
                throw new BadRequestException("send all arranges");
            }

            foreach (var item in categories)
            {
                var index = arrangeIds.IndexOf(item.Arrange);
                item.Arrange = index + 1;
            }

            _dbContext.Categories.UpdateRange(categories);

            await _dbContext.SaveChangesAsync();

            return categories
                .OrderBy(x => x.Arrange)
                .Select(s => new CategoryToReturnDTO
                {
                    CategoryId = s.Id,
                    ImageUrl_L = s.ImageUrl_L,
                    ImageUrl_M = s.ImageUrl_M,
                    ImageUrl_S = s.ImageUrl_S,
                    Level = s.Level,
                    Name = s.Name,
                    ArrangeId = s.Arrange
                }).ToList();
        }

        public async Task<List<CategoryToReturnDTO>> GetSubCategories(int id)
        {
            List<int> childCats = new();
            childCats = await MyPathToLeaf(childCats, id);

            return await _dbContext.Categories.Where(x => childCats.Contains(x.Id))
                .Select(s => new CategoryToReturnDTO
                {
                    Name = s.Name,
                    Level = s.Level,
                    ArrangeId = s.Arrange,
                    CategoryId = s.Id,
                    ImageUrl_L = s.ImageUrl_L,
                    ImageUrl_M = s.ImageUrl_M,
                    ImageUrl_S = s.ImageUrl_S
                })
                .ToListAsync();

        }

        public async Task<List<CategoryToReturnDTO>> GetParentCategories(int id)
        {
            List<int> childCats = new();
            childCats = await MyPathToRoot(childCats, id);

            return await _dbContext.Categories.Where(x => childCats.Contains(x.Id))
                .Select(s => new CategoryToReturnDTO
                {
                    Name = s.Name,
                    Level = s.Level,
                    ArrangeId = s.Arrange,
                    CategoryId = s.Id,
                    ImageUrl_L = s.ImageUrl_L,
                    ImageUrl_M = s.ImageUrl_M,
                    ImageUrl_S = s.ImageUrl_S
                })
                .ToListAsync();

        }
    }
}
