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
using Flurl.Http;
using Flurl;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Services
{
    public class PhoneSpecificationsApi : IPhoneSpecificationsApi
    {
        private readonly string _baseUrl;

        public PhoneSpecificationsApi(IOptions<PhoneSpecificationsApiOptions> options)
        {
            _baseUrl = options.Value.BaseUrl;
        }

        public async Task<LatestDto> GetLatestAsync(CancellationToken token)
        {
            try
            {
                var response = await _baseUrl.AppendPathSegments("v2", "latest").GetAsync(token);
                if (response.StatusCode == 200)
                {
                    var latestDto = await response.GetJsonAsync<LatestDto>();
                    if (latestDto.Status)
                    {
                        return latestDto;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public async Task<ListBrandsDto> GetListBrandsAsync(CancellationToken token)
        {
            try
            {
                var response = await _baseUrl.AppendPathSegments("v2", "brands").GetAsync(token);
                if (response.StatusCode == 200)
                {
                    var listBrandsDto = await response.GetJsonAsync<ListBrandsDto>();
                    if (listBrandsDto.Status)
                    {
                        return listBrandsDto;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public async Task<ListPhonesDto> GetListPhonesAsync(string brandSlug, int page, CancellationToken token)
        {
            try
            {
                var response = await _baseUrl.AppendPathSegments("v2", "brands", brandSlug)
                    .SetQueryParams(new {page = page})
                    .GetAsync(token);
                if (response.StatusCode == 200)
                {
                    var listPhonesDto = await response.GetJsonAsync<ListPhonesDto>();
                    if (listPhonesDto.Status)
                    {
                        return listPhonesDto;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public async Task<ListPhonesDto> GetListPhonesAsync2(string brandSlug, int page, CancellationToken token)
        {
            try
            {
                using var httpClient = new HttpClient();
                var httpResponse = await httpClient.GetAsync($"{_baseUrl}/v2/brands/{brandSlug}?page={page}", token);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var listPhonesDto =
                        await httpResponse.Content.ReadFromJsonAsync<ListPhonesDto>(cancellationToken: token);
                    if (listPhonesDto.Status)
                    {
                        return listPhonesDto;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public async Task<PhoneSpecificationsDto> GetPhoneSpecificationsAsync(string phoneSlug, CancellationToken token)
        {
            try
            {
                var response = await _baseUrl.AppendPathSegments("v2", phoneSlug).GetAsync(token);
                if (response.StatusCode == 200)
                {
                    var phoneSpecificationsDto = await response.GetJsonAsync<PhoneSpecificationsDto>();
                    if (phoneSpecificationsDto.Status)
                    {
                        return phoneSpecificationsDto;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public async Task<PhoneSpecificationsDto> GetPhoneSpecificationsAsync2(string phoneSlug,
            CancellationToken token)
        {
            try
            {
                using var httpClient = new HttpClient();
                var httpResponse = await httpClient.GetAsync($"{_baseUrl}/v2/{phoneSlug}", token);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var phoneSpecificationsDto = await httpResponse.Content.ReadFromJsonAsync<PhoneSpecificationsDto>(
                        cancellationToken: token);
                    if (phoneSpecificationsDto.Status)
                    {
                        return phoneSpecificationsDto;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public async Task<SearchDto> SearchAsync(string query, CancellationToken token)
        {
            try
            {
                var response = await _baseUrl.AppendPathSegments("v2", "search")
                    .SetQueryParams(new {query = query})
                    .GetAsync(token);
                if (response.StatusCode == 200)
                {
                    var searchDto = await response.GetJsonAsync<SearchDto>();
                    if (searchDto.Status)
                    {
                        return searchDto;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public async Task<TopByFansDto> GetTopByFansAsync(CancellationToken token)
        {
            try
            {
                var response = await _baseUrl.AppendPathSegments("v2", "top-by-fans").GetAsync(token);
                if (response.StatusCode == 200)
                {
                    var topByFansDto = await response.GetJsonAsync<TopByFansDto>();
                    if (topByFansDto.Status)
                    {
                        return topByFansDto;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public async Task<TopByInterestDto> GetTopByInterestAsync(CancellationToken token)
        {
            try
            {
                var response = await _baseUrl.AppendPathSegments("v2", "top-by-interest").GetAsync(token);
                if (response.StatusCode == 200)
                {
                    var topByInterestDto = await response.GetJsonAsync<TopByInterestDto>();
                    if (topByInterestDto.Status)
                    {
                        return topByInterestDto;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }
    }
}