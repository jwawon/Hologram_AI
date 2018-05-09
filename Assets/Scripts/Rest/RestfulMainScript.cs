using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Models;
using Proyecto26;
using System.Collections.Generic;
using LitJson;

public class RestfulMainScript : MonoBehaviour {
	public TextAsset jsonDataTest;
    private readonly string basePath = "https://jsonplaceholder.typicode.com";
	[SerializeField]
	string basePath_fromai;

	public void GetTextToJson(){
		LitJson.JsonData getData = LitJson.JsonMapper.ToObject(jsonDataTest.text);
		string name = getData["name"].ToString();
		int score = int.Parse(getData["score"].ToString());	
		Debug.Log("Name: " + name + "\t" + "Score: " + score);
	}

	// public void GetTestFromJson() {
	// 	LitJson.JsonData getData = LitJson.JsonMapper.ToObject(jsonDataTest.text);
	// 	string ss = getData["response"].ToString();
	// 	Model model = new Model();
	// 	model.response = ss;
		
	// 	ff = LitJson.getData.FromJson(ss, model);

	// }

	#if UNITY_EDITOR
    public void GetDataFromAi() {
        //sample 로 일단 http 처리를 하겠습니다.
        string question = "hello world";
        string userid = "1234567890";
        RestClient.Get(basePath_fromai + "/api/rest/v1.0/ask?question="+question+"&userid="+userid).Then (res => {
            //success 
            //Ask val = _responseProcess(res.text);
            // EditorUtility.DisplayDialog("Response",val, "OK");
			// Person person = JsonUtility.FromJson(res.text, Person.class);
            
			Debug.Log(res.GetType());
			Debug.Log(res.data);
			
			EditorUtility.DisplayDialog("Response", res.text, "OK");
			
			LitJson.JsonData getData = LitJson.JsonMapper.ToObject(res.text);
			
			string answer = getData[0]["response"]["answer"].ToString();
			int score = int.Parse(getData[0]["response"]["userid"].ToString());	
				
			Debug.Log("Answer: " + answer + "\t" + "userid: " + score);			
			
        }).Catch(err => EditorUtility.DisplayDialog("Error",err.Message,"OK"));
    }

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
	#endif
}