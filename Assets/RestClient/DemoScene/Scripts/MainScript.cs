using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Models;
using Proyecto26;
using System.Collections.Generic;
using LitJson;

public class MainScript : MonoBehaviour {

    private readonly string basePath = "https://jsonplaceholder.typicode.com";
	private readonly string basePath_fromai = "http://61.85.36.59";

	#if UNITY_EDITOR

	public void Get(){

		// We can add default request headers for all requests
		RestClient.DefaultRequestHeaders["Authorization"] = "Bearer ...";

        RequestHelper requestOptions = null;

		RestClient.GetArray<Post>(basePath + "/posts").Then(res => {
            EditorUtility.DisplayDialog ("Posts", JsonHelper.ArrayToJsonString<Post>(res, true), "Ok");
            return RestClient.GetArray<Todo>(basePath + "/todos");
		}).Then(res => {
            EditorUtility.DisplayDialog ("Todos", JsonHelper.ArrayToJsonString<Todo>(res, true), "Ok");
            return RestClient.GetArray<User>(basePath + "/users");
		}).Then(res => {
			EditorUtility.DisplayDialog ("Users", JsonHelper.ArrayToJsonString<User>(res, true), "Ok");


			// We can add specific options and override default headers for a request
			requestOptions = new RequestHelper { 
				url = basePath + "/photos",
				headers = new Dictionary<string, string> {
					{ "Authorization", "Other token..." }
				}
			};
			return RestClient.GetArray<Photo>(requestOptions);
		}).Then(res => {
			EditorUtility.DisplayDialog("Header", requestOptions.GetHeader("Authorization"), "Ok");

			// And later we can clean the default headers for all requests
			RestClient.CleanDefaultHeaders();

		}).Catch(err => EditorUtility.DisplayDialog ("Error", err.Message, "Ok"));
	}

	public void Post(){
		
		RestClient.Post<Models.Post>(basePath + "/posts", new {
			title = "foo",
			body = "bar",
			userId = 1
		})
		.Then(res => EditorUtility.DisplayDialog ("Success", JsonUtility.ToJson(res, true), "Ok"))
		.Catch(err => EditorUtility.DisplayDialog ("Error", err.Message, "Ok"));
	}

	public void Put(){

        RestClient.Put<Post>(basePath + "/posts/1", new {
			title = "foo",
			body = "bar",
			userId = 1
		}, (err, res, body) => {
			if(err != null){
				EditorUtility.DisplayDialog ("Error", err.Message, "Ok");
			}
			else{
				EditorUtility.DisplayDialog ("Success", JsonUtility.ToJson(body, true), "Ok");
			}
		});
	}

	public void Delete(){

        RestClient.Delete(basePath + "/posts/1", (err, res) => {
			if(err != null){
				EditorUtility.DisplayDialog ("Error", err.Message, "Ok");
			}
			else{
				EditorUtility.DisplayDialog ("Success", "Status: " + res.statusCode.ToString(), "Ok");
			}
		});
	}

	public void getDataFromNLP() {
		//sample 로 일단 http 처리를 하겠습니다.
		string question = "hello world";
		RestClient.Get(basePath_fromai + ":5000/api/rest/v1.0/ask?question="+question).Then (res => {
			//success 
			JsonData tmp_array_data = JsonMapper.ToObject(res.text);
			JsonData tmp_response_data = tmp_array_data[0];
			JsonData tmp_json_data = tmp_response_data["response"];
			string answer = (string) tmp_json_data["answer"];
			EditorUtility.DisplayDialog("Response", answer, "OK");
		}).Catch(err => EditorUtility.DisplayDialog("Error",err.Message,"OK"));
	}

	public void getDataFromCV() {
		//sample 로 일단 http 처리를 하겠습니다.

	}
	#endif

}