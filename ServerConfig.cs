public class ServerConfig
{
    private string SERVER_URL = "https://vr-climate-api.herokuapp.com";
    private string SERVER_ENDPOINT = "/decade?";
    private string YEAR = "year";
    private string SCENARIO = "scenario";

    public string URL
    { get { return SERVER_URL; } }

    public string DECADE_URL
    { get { return SERVER_URL+SERVER_ENDPOINT; } }

    public string QUERY_YEAR
    { get { return make_query(YEAR); } }

    public string QUERY_SCENARIO
    { get { return make_query(SCENARIO); } }

    private make_query(string param) {
        return param+"=";
    }
}