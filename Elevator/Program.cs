// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

var client = new HttpClient();
client.BaseAddress = new Uri("http://involved-htf-challenge.azurewebsites.net");

var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiNDQiLCJuYmYiOjE2Mzg0MzcwMzMsImV4cCI6MTYzODUyMzQzMywiaWF0IjoxNjM4NDM3MDMzfQ.unASlknxxtCrp10ad9SWlBJDrRe8uaeKoZeTFCyz03FkPTpaaYMJE6j-s1kFJsVUaPB7XrotGciLYfuv2RyIRg";

client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

var startUrl = "api/path/1/medium/Start";

var startResponse = await client.GetAsync(startUrl);

var sampleUrl = "/api/path/1/medium/Sample";

var sampleGetResponse = await client.GetFromJsonAsync<InputObject>(sampleUrl);

Console.WriteLine(sampleGetResponse);

var sampleAnswer = GetAnswer(sampleGetResponse.Start, sampleGetResponse.Destination);

foreach (var floor in sampleAnswer)
{
    Console.WriteLine(floor);
}


var samplePostResponse = await client.PostAsJsonAsync(sampleUrl, sampleAnswer);

var samplePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();
Console.WriteLine(samplePostResponseValue);

var puzzleUrl = "/api/path/1/medium/Puzzle";

var puzzleGetResponse = await client.GetFromJsonAsync<InputObject>(puzzleUrl);
Console.WriteLine(puzzleGetResponse);
// Je zoekt het antwoord
var puzzleAnswer = GetAnswer(puzzleGetResponse.Start, puzzleGetResponse.Destination);
Console.WriteLine(puzzleAnswer);

var puzzlePostResponse = await client.PostAsJsonAsync(puzzleUrl, puzzleAnswer);

var puzzlePostResponseValue = await puzzlePostResponse.Content.ReadAsStringAsync();
Console.WriteLine(puzzlePostResponseValue);

List<int> GetAnswer(int start, int destination)
{
    int floor = 0;
    List<int> trail = new List<int>();
    trail.Add(start);
    Walker(trail, floor, destination, true, 0);
    trail.RemoveAt(trail.Count - 1);
    return trail;
}

void Walker(List<int> floorTrail, int currentFloor, int requiredFloor, bool directionFlag, int step)
{
    if (currentFloor == requiredFloor)
    {
        floorTrail.Add(currentFloor);
    }
    else
    {
        step++;
        currentFloor = (directionFlag)?floorTrail.Last() + step:floorTrail.Last() - step;
        floorTrail.Add(currentFloor);
        
        if (currentFloor > requiredFloor) //result too high
        {
            floorTrail.RemoveAt(floorTrail.Count - 1); //remove last
            Walker(floorTrail, floorTrail.Last(), requiredFloor, false, --step );
        }
        else
        {
            Walker(floorTrail, floorTrail.Last(), requiredFloor, true, step );
        }
    }
}

class InputObject
{
    public int Start { get; set; }
    public int Destination { get; set; }
}