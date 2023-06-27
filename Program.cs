using EspacioMoneda;
using System.Net;
using System.Text.Json;

internal class Program {
  private static void Main(string[] args) {
    Root response = doRequest("https://api.coindesk.com/v1/bpi/currentprice.json");

    if (response.bpi != null) {
      mostrarDetallesMonedaAEleccion(response);
    } else {
      Console.WriteLine("x Error, endpoint no devolvió respuesta.");
    }
  }

  static private void mostrarDetallesMonedaAEleccion(Root monedaResponse) {
    int decision = mostrarMonedasDeuelveDecision();

    switch(decision) {
      case 1:
        Console.WriteLine("- Code: " + monedaResponse.bpi.USD.code);
        Console.WriteLine("- Description: " + monedaResponse.bpi.USD.description);
        Console.WriteLine("- Rate: " + monedaResponse.bpi.USD.rate);
        Console.WriteLine("- Rate_float: " + monedaResponse.bpi.USD.rate_float);
        Console.WriteLine("- Symbol: " + monedaResponse.bpi.USD.symbol);
        break;
      case 2:
        Console.WriteLine("- Code: " + monedaResponse.bpi.GBP.code);
        Console.WriteLine("- Description: " + monedaResponse.bpi.GBP.description);
        Console.WriteLine("- Rate: " + monedaResponse.bpi.GBP.rate);
        Console.WriteLine("- Rate_float: " + monedaResponse.bpi.GBP.rate_float);
        Console.WriteLine("- Symbol: " + monedaResponse.bpi.GBP.symbol);
        break;
      case 3:
        Console.WriteLine("- Code: " + monedaResponse.bpi.EUR.code);
        Console.WriteLine("- Description: " + monedaResponse.bpi.EUR.description);
        Console.WriteLine("- Rate: " + monedaResponse.bpi.EUR.rate);
        Console.WriteLine("- Rate_float: " + monedaResponse.bpi.EUR.rate_float);
        Console.WriteLine("- Symbol: " + monedaResponse.bpi.EUR.symbol);
        break;
      default:
        Console.WriteLine("x Opción invalida, por favor reintente.");
        break;
    }
  }

  static private int mostrarMonedasDeuelveDecision() {
    Console.WriteLine("Elija de que moneda quiere ver detalles:");
    Console.WriteLine(" 1 - USD");
    Console.WriteLine(" 2 - GBP");
    Console.WriteLine(" 3 - EUR");

    int decision;

    if (
      !int.TryParse(Console.ReadLine(), out decision) ||
      decision < 1 ||
      decision > 3
    ) {
      Console.Clear();
      Console.WriteLine("x Opción invalida, por favor reintente.");
      return mostrarMonedasDeuelveDecision();
    }

    return decision;
  }
  
  static private Root doRequest(string url) {
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    request.Method = "GET";
    request.ContentType = "application/json";
    request.Accept = "application/json";
    
    try {
        using (WebResponse response = request.GetResponse()) {
            using (Stream strReader = response.GetResponseStream()) {
                if (strReader == null) return new Root();

                using (StreamReader objReader = new StreamReader(strReader)) {
                    string responseBody = objReader.ReadToEnd();

                    if (string.IsNullOrEmpty(responseBody)) return new Root();

                    return JsonSerializer.Deserialize<Root>(responseBody);
                }
            }
        }
    } catch (WebException ex) {
        Console.WriteLine("Problemas de acceso a la API");

        return new Root();
    }
  }
}
