

  /*

    http://procbits.com/2013/08/27/generating-a-bitcoin-address-with-javascript/
    https://www.johndcook.com/blog/2018/08/14/bitcoin-elliptic-curves/

   
  Serial.println("Verified:");
  char * data = "Hello World!";
  Serial.println("7f83b1657ff1fc53b92dc18148a1d65dfc2d4b1fa3d677284addd200126d9069");
  Serial.println();


  Serial.println("Int values:");
  uint8_t hashInt[64];
  hashArrayInt(hashInt, data);
  
  Serial.println(hashInt[0]);
  Serial.println(hashInt[1]);
  Serial.println(hashInt[2]);
  Serial.println();

  
  Serial.println("Hexed values:");
  char hex[64];
  toHex(hex, hashInt, 32);
  
  Serial.println(hex);
  Serial.println();
  
  
  Serial.println("Full function:");
  char hashHex[64];
  hashArrayHex(hashHex, data);
  
  Serial.println(hashHex);
  Serial.println();

  
void hashArrayInt(uint8_t *dest, char *data) {
  
  uint8_t value[32];
  
  sha256.reset();
  sha256.update(data, strlen(data));
  sha256.finalize(value, sizeof(value));
  
  memcpy(dest, value, 32);
}

void hashArrayHex(char *dest, char *data) {
  
  uint8_t value[32];
  char hex[64];
  
  sha256.reset();
  sha256.update(data, strlen(data));
  sha256.finalize(value, sizeof(value));
  
  toHex(dest, value, 32);
}

void toHex(char *dest, uint8_t *src, int len) {
  uint8_t *d = dest;
  while( len-- ) {
    sprintf(d, "%02x", (unsigned char)*src++);
    d += 2;
  }
}
*/
