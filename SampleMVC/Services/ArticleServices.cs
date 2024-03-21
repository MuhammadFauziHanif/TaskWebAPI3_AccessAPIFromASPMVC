using MyRESTServices.BLL.DTOs;
using System.Text;
using System.Text.Json;

namespace SampleMVC.Services
{
    public class ArticleServices : IArticleServices
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ArticleServices> _logger;

        public ArticleServices(HttpClient client, IConfiguration configuration, ILogger<ArticleServices> logger)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
        }

        private string GetBaseUrl()
        {
            return _configuration["BaseUrl"] + "/Articles";
        }

        public async Task<IEnumerable<ArticleDTO>> GetAll()
        {
            _logger.LogInformation(GetBaseUrl());
            var httpResponse = await _client.GetAsync(GetBaseUrl());

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve article");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<IEnumerable<ArticleDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return categories;
        }

        public async Task<ArticleDTO> GetArticleById(int id)
        {
            var httpResponse = await _client.GetAsync($"{GetBaseUrl()}/{id}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve article");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var article = JsonSerializer.Deserialize<ArticleDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return article;
        }

        //post
        public async Task<ArticleDTO> Insert(ArticleCreateDTO articleCreateDTO)
        {
            var json = JsonSerializer.Serialize(articleCreateDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(GetBaseUrl(), data);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot insert article");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var article = JsonSerializer.Deserialize<ArticleDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return article;
        }

        //put
        public async Task<ArticleDTO> Update(int id, ArticleUpdateDTO article)
        {
            var json = JsonSerializer.Serialize(article);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync($"{GetBaseUrl()}/{id}", data);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot update article");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var articleUpdated = JsonSerializer.Deserialize<ArticleDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return articleUpdated;
        }

        //delete
        public async Task<bool> Delete(int id)
        {
            var httpResponse = await _client.DeleteAsync($"{GetBaseUrl()}/{id}");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot delete article");
            }

            return true;

        }

        public Task<IEnumerable<ArticleDTO>> GetArticleWithCategory()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArticleDTO>> GetArticleByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertWithIdentity(ArticleCreateDTO article)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArticleDTO>> GetWithPaging(int categoryId, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountArticles()
        {
            throw new NotImplementedException();
        }
    }
}
