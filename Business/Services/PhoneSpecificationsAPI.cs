using System;
using Application.DTO.Options;
using Application.DTO.PhoneSpecificationsAPI.Latest;
using Application.DTO.PhoneSpecificationsAPI.ListBrands;
using Application.DTO.PhoneSpecificationsAPI.ListPhones;
using Application.DTO.PhoneSpecificationsAPI.PhoneSpecifications;
using Application.DTO.PhoneSpecificationsAPI.Search;
using Application.DTO.PhoneSpecificationsAPI.TopByFans;
using Application.DTO.PhoneSpecificationsAPI.TopByInterest;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Application.DTO.PhoneSpecificationsAPI;

namespace Application.Services
{
    public class PhoneSpecificationsApi : IPhoneSpecificationsApi
    {
        private readonly string _baseUrl;

        public PhoneSpecificationsApi(IOptions<PhoneSpecificationsApiOptions> options)
        {
            _baseUrl = options.Value.BaseUrl;
        }

        private async Task<ApiResponseDto> HttpClientGetRequestAsync<T>(string url, CancellationToken token)
        {
            var apiResponseDto = new ApiResponseDto();
            try
            {
                using var httpClient = new HttpClient();
                var httpResponse = await httpClient.GetAsync(url, token);

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    apiResponseDto.Message = "HttpClient Error: Wrong StatusCode";
                    apiResponseDto.StatusCode = httpResponse.StatusCode;
                    return apiResponseDto;
                }

                apiResponseDto.Message = "Ok";
                apiResponseDto.StatusCode = HttpStatusCode.OK;
                apiResponseDto.Data = await httpResponse.Content.ReadFromJsonAsync<T>(cancellationToken: token);
            }
            catch (Exception)
            {
                apiResponseDto.Message = "HttpClient Error: Exception";
                apiResponseDto.StatusCode = HttpStatusCode.BadRequest;
            }

            return apiResponseDto;
        }

        public async Task<ApiResponseDto> GetListBrandsAsync(CancellationToken token)
        {
            var url = $"{_baseUrl}/v2/brands";
            return await HttpClientGetRequestAsync<ListBrandsDto>(url, token);
        }

        public async Task<ApiResponseDto> GetListPhonesAsync(string brandSlug, int page, CancellationToken token)
        {
            var url = $"{_baseUrl}/v2/brands/{brandSlug}?page={page}";
            return await HttpClientGetRequestAsync<ListPhonesDto>(url, token);
        }

        public async Task<ApiResponseDto> GetPhoneSpecificationsAsync(string phoneSlug, CancellationToken token)
        {
            var url = $"{_baseUrl}/v2/{phoneSlug}";
            return await HttpClientGetRequestAsync<PhoneSpecificationsDto>(url, token);
        }

        public async Task<ApiResponseDto> SearchAsync(string query, CancellationToken token)
        {
            var url = $"{_baseUrl}/v2/search?query={query}";
            return await HttpClientGetRequestAsync<SearchDto>(url, token);
        }

        public async Task<ApiResponseDto> GetLatestAsync(CancellationToken token)
        {
            var url = $"{_baseUrl}/v2/latest";
            return await HttpClientGetRequestAsync<LatestDto>(url, token);
        }

        public async Task<ApiResponseDto> GetTopByFansAsync(CancellationToken token)
        {
            var url = $"{_baseUrl}/v2/top-by-fans";
            return await HttpClientGetRequestAsync<TopByFansDto>(url, token);
        }

        public async Task<ApiResponseDto> GetTopByInterestAsync(CancellationToken token)
        {
            var url = $"{_baseUrl}/v2/top-by-interest";
            return await HttpClientGetRequestAsync<TopByInterestDto>(url, token);
        }
    }
}