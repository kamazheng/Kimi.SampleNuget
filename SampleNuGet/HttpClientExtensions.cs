// ***********************************************************************
// Author           : MOLEX\kzheng
// Created          : 01/14/2025
// ***********************************************************************

using System.Net.Http.Json;

namespace BlazorWebAppOidc.Client.Infrastructure;
public static class HttpClientExtensions
{
	/// <summary>
	/// Sends a POST request to the specified URL with the provided request object.
	/// </summary>
	/// <typeparam name="TRequest">The type of the request object.</typeparam>
	/// <param name="httpClient">The HttpClient instance.</param>
	/// <param name="url">The URL to send the request to.</param>
	/// <param name="request">The request object.</param>
	/// <returns>A boolean value indicating whether the request was successful.</returns>
	public static async Task<bool> PostAsync<TRequest>(this HttpClient httpClient, string url, TRequest request)
	{
		var response = await httpClient.PostAsJsonAsync(url, request);
		response.EnsureSuccessCode();
		return true;
	}

	/// <summary>
	/// Sends a POST request to the specified URL with the provided request object and returns the response object.
	/// </summary>
	/// <typeparam name="TRequest">The type of the request object.</typeparam>
	/// <typeparam name="TResponse">The type of the response object.</typeparam>
	/// <param name="httpClient">The HttpClient instance.</param>
	/// <param name="url">The URL to send the request to.</param>
	/// <param name="request">The request object.</param>
	/// <returns>The response object.</returns>
	public static async Task<TResponse?> PostAsync<TRequest, TResponse>(this HttpClient httpClient, string url, TRequest request)
	{
		var response = await httpClient.PostAsJsonAsync(url, request);
		response.EnsureSuccessCode();
		return await response.Content.ReadFromJsonAsync<TResponse>();
	}

	/// <summary>
	/// Sends a POST request to the specified URL with the provided request object and returns the response object.
	/// If the response object is null, throws an HttpRequestException.
	/// </summary>
	/// <typeparam name="TRequest">The type of the request object.</typeparam>
	/// <typeparam name="TResponse">The type of the response object.</typeparam>
	/// <param name="httpClient">The HttpClient instance.</param>
	/// <param name="url">The URL to send the request to.</param>
	/// <param name="request">The request object.</param>
	/// <returns>The response object.</returns>
	/// <exception cref="HttpRequestException">Thrown when the response object is null.</exception>
	public static async Task<TResponse> PostRequiredResponseAsync<TRequest, TResponse>(this HttpClient httpClient, string url, TRequest request)
	{
		var response = await httpClient.PostAsJsonAsync(url, request);
		response.EnsureSuccessCode();
		var result = await response.Content.ReadFromJsonAsync<TResponse>();
		if (result == null)
		{
			throw new HttpRequestException("No response from server.");
		}
		return result;
	}

	/// <summary>
	/// Ensures that the HTTP response has a successful status code.
	/// If the response status code is not successful, throws an HttpRequestException.
	/// </summary>
	/// <param name="response">The HttpResponseMessage instance.</param>
	/// <exception cref="HttpRequestException">Thrown when the response status code is not successful.</exception>
	public static void EnsureSuccessCode(this HttpResponseMessage response)
	{
		if (!response.IsSuccessStatusCode)
		{
			string content = response.Content.ReadAsStringAsync().Result;
			throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Response content: {content}");
		}
	}
}
