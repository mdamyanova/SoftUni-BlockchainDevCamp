import org.json.simple.JSONObject;

public class EthereumSignatureToAddress {

    public static void main(String[] args) throws Exception {
        // Input - JSON object
        JSONObject result = new JSONObject();
        result.put(
                "signature",
                "0xacd0acd4eabd1bec05393b33b4018fa38b69eba8f16ac3d60eec9f4d2abc127e3c92939e680b91b094242af80fce6f217a34197a69d35edaf616cb0c3da4265b01");
        result.put("v", "0x1");
        result.put(
                "r", "0xacd0acd4eabd1bec05393b33b4018fa38b69eba8f16ac3d60eec9f4d2abc127e");
        result.put(
                "s", "0x3c92939e680b91b094242af80fce6f217a34197a69d35edaf616cb0c3da4265b");

        // Recover public key


        // Output - Ethereum address

    }
}