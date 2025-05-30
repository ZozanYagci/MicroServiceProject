﻿using WebApp.Application.Services.Interfaces;
using WebApp.Domain.Models;
using WebApp.Domain.Models.CatalogModels;
using WebApp.Extensions;

namespace WebApp.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient apiClient;

        public CatalogService(HttpClient apiClient)
        {
            this.apiClient = apiClient;
        }
        public async Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItems()
        {
            var response = await apiClient.GetReponseAsync<PaginatedItemsViewModel<CatalogItem>>("/catalog/items");
            return response;
        }
    }
}
