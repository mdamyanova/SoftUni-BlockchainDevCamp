import org.web3j.crypto.Sign;

import java.math.BigInteger;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;

import static java.nio.charset.Charset.*;

public class EthereumSignatureToAddress {

    public static void main(String[] args) throws Exception {
        // Input - JSON object
        // TODO
        //JSONObject result = new JSONObject();
        //result.put(
        //        "signature",
        //        "0xacd0acd4eabd1bec05393b33b4018fa38b69eba8f16ac3d60eec9f4d2abc127e3c92939e680b91b094242af80fce6f217a34197a69d35edaf616cb0c3da4265b01");
        //result.put("v", "0x1");
        //result.put(
        //       "r", "0xacd0acd4eabd1bec05393b33b4018fa38b69eba8f16ac3d60eec9f4d2abc127e");
        //result.put(
        //        "s", "0x3c92939e680b91b094242af80fce6f217a34197a69d35edaf616cb0c3da4265b");


        // More readable input
        String message = "exercise-cryptography";
        byte v = 27;
        String r = "acd0acd4eabd1bec05393b33b4018fa38b69eba8f16ac3d60eec9f4d2abc127e";
        String s = "3c92939e680b91b094242af80fce6f217a34197a69d35edaf616cb0c3da4265b";

        // Recover public key
        Sign.SignatureData signature = new Sign.SignatureData(v, hexStringToByteArray(r), hexStringToByteArray(s));
        BigInteger key = Sign.signedMessageToKey(message.getBytes(), signature);

        // Output - Ethereum address
        System.out.println(key.toString(16));
    }

    private static byte[] hexStringToByteArray(String s) {
        int len = s.length();
        byte[] data = new byte[len / 2];
        for (int i = 0; i < len; i += 2) {
            data[i / 2] = (byte) ((Character.digit(s.charAt(i), 16) << 4)
                    + Character.digit(s.charAt(i+1), 16));
        }
        return data;
    }
}