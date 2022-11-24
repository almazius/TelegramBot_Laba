using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public struct Joke
    {
        public bool error;
        public int code;
        public string category;
        public string type;
        public string setup;
        public string delivery;
        public string joke;
        public int id;

    }
    internal class LoveChecker
    {
        public static async Task<string> GetData(string name1, string name2)
        {
            string result;
            if (name1 == "c" || name2 == "c" || name1 == "go" || name2 == "go")
            {
                result = "Your percentage: 97%\nC/C++/Go best couple in your life\n";
            }
            else if (name1 == "java" || name2 == "java")
            {
                result = "Your percentage: 2%\n" + GetJoke("java").Result;
            }
            else
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://love-calculator.p.rapidapi.com/getPercentage?sname={name1}&fname={name2}"),
                    Headers =
    {
        { "X-RapidAPI-Key", "0977426b4emshe80eaae046a21aap12c719jsn93af36aca707" },
        { "X-RapidAPI-Host", "love-calculator.p.rapidapi.com" },
    },
                };
                using (var response = await client.SendAsync(request))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        string[] newBody = body.Split(',');
                        result = $"Your percentage: {newBody[2].Substring(14, (Char.IsDigit(newBody[2][15]) ? 2 : 1))}%\n" +
                            $"{newBody[3].Substring(10, newBody[3].Length - 9 - 3)}";
                    }
                    catch (Exception e)
                    {
                        result = $"ERROR\n {e.Message}";
                        throw;
                    }
                    

                }
            }
            return result;
        }

        public static async Task<string?> GetJoke(string jokeName="")
        {
            string result;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://v2.jokeapi.dev/joke/Any?contains={jokeName}"),
            };
            try
            {
                using (var response = await client.SendAsync(request))
                {

                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Joke joke = JsonConvert.DeserializeObject<Joke>(body);
                    if(joke.error == true && joke.code == 106)
                    {
                        result = "I can't found joke 😢";
                    } else if(joke.error == true)
                    {
                        result = "Error on joke 😢";
                    }
                    else
                    {
                        if (joke.type == "single")
                        {
                            result = joke.joke;
                        }
                        else
                        {
                            result = joke.setup + '\n' + joke.delivery;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return ($"Error\n{e.Message}");
                throw;
            }
            return result;
        }
    }
}
