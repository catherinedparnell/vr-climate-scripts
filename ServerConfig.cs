public class ServerConfig
{
    private static string SERVER_URL = "https://vr-climate-api.herokuapp.com/?";
    private static string SERVER_ENDPOINT = "/decade?";
    private static string YEAR = "year";
    private static string SCENARIO = "scenario";

    public static string URL
    { get { return SERVER_URL; } }

    public static string DECADE_URL
    { get { return SERVER_URL+SERVER_ENDPOINT; } }

    public static string QUERY_YEAR
    { get { return make_query(YEAR); } }

    public static string QUERY_SCENARIO
    { get { return make_query(SCENARIO); } }

    private static string make_query(string param) {
        return param+"=";
    }
}