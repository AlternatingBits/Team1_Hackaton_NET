// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

var client = new HttpClient();
client.BaseAddress = new Uri("http://involved-htf-challenge.azurewebsites.net");

var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiNDQiLCJuYmYiOjE2Mzg0MzcwMzMsImV4cCI6MTYzODUyMzQzMywiaWF0IjoxNjM4NDM3MDMzfQ.unASlknxxtCrp10ad9SWlBJDrRe8uaeKoZeTFCyz03FkPTpaaYMJE6j-s1kFJsVUaPB7XrotGciLYfuv2RyIRg";

client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

var startUrl = "api/path/2/easy/Start";

var startResponse = await client.GetAsync(startUrl);

var sampleUrl = "/api/path/2/easy/Sample";

var sampleGetResponse = await client.GetFromJsonAsync<DateObject>(sampleUrl);
Console.WriteLine(sampleGetResponse);
Console.WriteLine(FormatDatetime(sampleGetResponse.Date1).ToShortDateString());
Console.WriteLine(GetAnswer(sampleGetResponse));

// Je zoekt het antwoord
var sampleAnswer = GetAnswer(sampleGetResponse);

// We sturen het antwoord met een POST request
// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
// De response die we krijgen zal ons zeggen of ons antwoord juist was
var samplePostResponse = await client.PostAsJsonAsync(sampleUrl, sampleAnswer);

var samplePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();
Console.WriteLine(samplePostResponseValue);

/*var puzzleUrl = "/api/path/2/easy/Puzzle";

var puzzleGetResponse = await client.GetFromJsonAsync<DateObject>(puzzleUrl);
Console.WriteLine(puzzleGetResponse);
// Je zoekt het antwoord
var puzzleAnswer = GetAnswer(puzzleGetResponse);
Console.WriteLine(puzzleAnswer);

var puzzlePostResponse = await client.PostAsJsonAsync<double>(puzzleUrl, puzzleAnswer);

var puzzlePostResponseValue = await puzzlePostResponse.Content.ReadAsStringAsync();
Console.WriteLine(puzzlePostResponseValue);*/

double GetAnswer(DateObject date)
{
    return Math.Abs((FormatDatetime(date.Date1) - FormatDatetime(date.Date2)).TotalSeconds);
}

DateTime FormatDatetime(string date)
{
    int day = 0, month = 0, year = 0, hour = 0, minute = 0, second = 0;
    StringBuilder placeholder = new StringBuilder(String.Empty);
    foreach (var character in date)
    {
        if (char.IsNumber(character))
            placeholder = placeholder.Append(character);
        else if(placeholder.Length != 0)
        {
            switch(character)
            {
                case 'D':
                    day = ToInt(placeholder);
                    break;
                case 'M':
                    month = ToInt(placeholder);
                    break;
                case 'Y':
                    year = ToInt(placeholder);
                    break;
                case 'h':
                    hour = ToInt(placeholder);
                    break;
                case 'm':
                    minute = ToInt(placeholder);
                    break;
                case 's':
                    second = ToInt(placeholder);
                    break;
                default:
                    break;
            }
                
            placeholder.Clear();
        }
    }
        
    return new DateTime(year, month, day, hour, minute, second);
}

int ToInt(StringBuilder input)
{
    return int.Parse(input.ToString());
}

class DateObject {
    public string Date1 { get; set; }
    public string Date2 { get; set; }

    public override string ToString()
    {
        return $"Date1: {Date1} Date2: {Date2}";
    }
}