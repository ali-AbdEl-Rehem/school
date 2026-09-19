using System.Net;
using System.Net.Http.Json;
using TeachersAffairs.Shared.Abstractions;
using TeachersAffairs.Shared.DTOs;

namespace TeachersAffairs.Web.Client.Services;

public sealed class HttpTeacherApiClient : ITeacherApi
{
    private const string Root = "api/teachers";
    private readonly HttpClient _http;

    public HttpTeacherApiClient(HttpClient http) => _http = http;

    public async Task<PagedResult<TeacherDto>> GetPagedAsync(TeacherQuery query, CancellationToken cancellationToken = default)
        => await _http.GetFromJsonAsync<PagedResult<TeacherDto>>($"{Root}?{query.ToQueryString()}", cancellationToken)
           ?? new PagedResult<TeacherDto>();

    public async Task<IReadOnlyList<TeacherDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _http.GetFromJsonAsync<List<TeacherDto>>($"{Root}/all", cancellationToken) ?? [];

    public async Task<TeacherDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _http.GetAsync($"{Root}/{id}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TeacherDto>(cancellationToken);
    }

    public async Task<TeacherDto> CreateAsync(TeacherCreateDto dto, CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync(Root, dto, cancellationToken);
        await EnsureSuccessOrThrowAsync(response, cancellationToken);
        return (await response.Content.ReadFromJsonAsync<TeacherDto>(cancellationToken))!;
    }

    public async Task<TeacherDto?> UpdateAsync(int id, TeacherUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var response = await _http.PutAsJsonAsync($"{Root}/{id}", dto, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        await EnsureSuccessOrThrowAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TeacherDto>(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _http.DeleteAsync($"{Root}/{id}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        await EnsureSuccessOrThrowAsync(response, cancellationToken);
        return true;
    }

    public async Task<IReadOnlyList<string>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
        => await _http.GetFromJsonAsync<List<string>>($"{Root}/departments", cancellationToken) ?? [];

    private static async Task EnsureSuccessOrThrowAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        string? detail = null;
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsLite>(cancellationToken);
            detail = problem?.Detail ?? problem?.Title;
        }
        catch
        {
        }

        throw new HttpRequestException(detail ?? $"Request failed with status {(int)response.StatusCode}.");
    }

    private sealed record ProblemDetailsLite(string? Title, string? Detail, int? Status);
}