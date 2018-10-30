using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL;
using WordPressPCL.Models;
using WordPressPCL.Utility;

namespace PlanMy.Library
{
    public class WordpressService
    {
        private readonly WordPressClient _client;

        public WordpressService()
        {
            _client = new WordPressClient(Statics.WordpressUrl);
        }

        public async Task<IEnumerable<Post>> GetLatestPostsAsync(int page = 0, int perPage = 20)
        {
            page++;

            var posts = await _client.Posts.Query(new PostsQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });

            return posts;
        }
        public async Task<IEnumerable<Item>> GetFeaturedItemsAsync(int page = 0, int perPage = 20)
        {
            page++;

            var posts = await _client.Items.Query(new PostsQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true,
                OrderBy = PostsOrderBy.Id,
                featured_item = true
            });
            //posts = posts.Skip((page - 1) * perPage).Take(perPage);//.Where(x => ((Dictionary<string,string>)x.Meta)["featured_item"]=="on");
            return posts;
        }
        public async Task<IEnumerable<Item>> GetItemsByCategoryAsync(int categoryId, int page = 0, int perPage = 10)
        {
            page++;
            int[] cats = new int[1];
            cats[0] = categoryId;
            var posts = await _client.Items.Query(new PostsQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true,
                ItemCategories = cats,
                OrderBy = PostsOrderBy.Id
            });
            //posts = posts.Skip((page - 1) * perPage).Take(perPage);//.Where(x => ((Dictionary<string,string>)x.Meta)["featured_item"]=="on");
            return posts;
        }
        public async Task<Item> GetItemByIDAsync(int id)
        {
            var posts = await _client.Items.GetByID(id);
            //posts = posts.Skip((page - 1) * perPage).Take(perPage);//.Where(x => ((Dictionary<string,string>)x.Meta)["featured_item"]=="on");
            return posts;
        }
        public async Task<IEnumerable<Item>> GetItemsByFilterAsync(int categoryId, int[] ItemTypes, int[] HoneymoonExperience, int[] TypeOfService, int[] Capacity, int[] ItemSetting, int[] ItemCateringServices, int[] ItemtypeOfFurniture, int[] ItemClientele, int[] ItemClothing, int[] ItemBeautyServices, int[] ItemTypeOfMusicians, int[] ItemCity, int page = 0, int perPage = 100)
        {
            page++;
            int[] cats = new int[1];
            cats[0] = categoryId;
            var posts = await _client.Items.Query(new PostsQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true,
                ItemCategories = cats,
                ItemTypes = ItemTypes,
                HoneymoonExperience = HoneymoonExperience,
                TypeOfService = TypeOfService,
                Capacity = Capacity,
                ItemSetting = ItemSetting,
                ItemCateringServices = ItemCateringServices,
                ItemtypeOfFurniture = ItemtypeOfFurniture,
                ItemClientele = ItemClientele,
                ItemClothing = ItemClothing,
                ItemBeautyServices = ItemBeautyServices,
                ItemTypeOfMusicians = ItemTypeOfMusicians,
                ItemCity = ItemCity,
                OrderBy = PostsOrderBy.Id
            });
            //posts = posts.Skip((page - 1) * perPage).Take(perPage);//.Where(x => ((Dictionary<string,string>)x.Meta)["featured_item"]=="on");
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemCategoriesAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemCategories.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemTypesAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemTypes.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemCitiesAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemCities.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemSettingsAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemSettings.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetCapacitiesAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.Capacities.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemCateringServicesAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemCateringServices.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemTypeOfFurnituresAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemTypeOfFurnitures.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemClientelesAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemClienteles.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemClothingsAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemClothings.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemBeautyServicesAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemBeautyServices.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemTypeOfMusiciansAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemTypeOfMusicians.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetHoneymoonExperiencesAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.HoneymoonExperiences.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetTypeOfServicesAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.TypeOfServices.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<IEnumerable<ItemCategory>> GetItemLocationsAsync(int page = 0, int perPage = 50)
        {
            page++;

            var posts = await _client.ItemLocations.Query(new CategoriesQueryBuilder
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
            return posts;
        }
        public async Task<List<CommentThreaded>> GetCommentsForPostAsync(int postid)
        {
            var comments = await _client.Comments.Query(new CommentsQueryBuilder
            {
                Posts = new[] { postid },
                Page = 1,
                PerPage = 100
            });

            return ThreadedCommentsHelper.GetThreadedComments(comments);
        }
    }
}
