namespace CampingWebsiteMVC.Services
{
    public interface ICampingWebsiteApiService
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, object data);
        Task<HttpResponseMessage> PutAsync(string url, object data);
    }
}

