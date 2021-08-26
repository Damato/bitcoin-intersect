

  /*
void exa() {
  //char x[2] = {'9', '0'};
  //Serial.println(strtoul( x, NULL, 16));

  char text[] = "801184CD2CDD640CA42CFC3A091C51D549B2F016D454B2774019C2B2D2E08529FD";
  char *firstSHA = toHash(text);
  char *firstSHAUpper = toUpperArray(firstSHA);
  Serial.println(firstSHAUpper);
  
  char *secondSHA = toHash(firstSHAUpper);
  char *secondSHAUpper = toUpperArray(secondSHA);
  
  Serial.println(secondSHAUpper);
  //Serial.println(stringToHex(text));
  
  Serial.println("Got:");
  char *data = "Hello World!";
  char *hash = toHash(data);
  Serial.println("7f83b1657ff1fc53b92dc18148a1d65dfc2d4b1fa3d677284addd200126d9069");
  Serial.println(toUpperArray(hash));

  Serial.println("Priv:");
  char *priv = privateKey(wiff);
  Serial.println(priv);

  //Serial.println(toHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", value, 32);
  
  Serial.println("Done");
}

char *stringToHex(char *w) {

  uint8_t output[32];
  int len = strlen(w);
  
  Serial.print("len:");
  Serial.println(len);
  
  for(int c = 0; c < len; c += 2) {
    
    Serial.print("char:");
    Serial.print(c);
    Serial.print(":val:");
    Serial.println((char)w[c]);
    
    char x[2] = { w[c], w[c+1] };
    Serial.println(x);
    output[c / 2] = strtoul(x, NULL, 16);
    Serial.println(output[c / 2]);
  }
  return w;
}

char *privateKey(char *w) {
  
  uint8_t randomized[32];
  for(int r = 0; r < 32; r++) {
    randomized[r] = (uint8_t)random(256);
  }
  
  char *hexed = toHex(hex, randomized, 32);
  char *uppered = toUpperArray(hexed);
  Serial.println(uppered);
  
  char *prefixed = w;
  prefixed[0] = '8';
  prefixed[1] = '0';
  
  int len = strlen(uppered);
  for(int c = 0; c < len; c++) {
    prefixed[c + 2] = uppered[c];
  }
  prefixed[66] = '\0';
    
  return w;
}

char *toUpperArray(char *s) {
  int len = strlen(s);
  for(int c = 0; c < len; c++) {
    s[c] = toupper(s[c]);
  }
  return s;
}

char toUpper(char c) {
  return toupper(c);
}

uint8_t *hashArrayInt(char *data) {
  
  uint8_t value[32];
  
  sha256.reset();
  sha256.update(data, strlen(data));
  sha256.finalize(value, sizeof(value));
  
  return value;
}

char *hashArrayHex(char *data) {
  
  uint8_t value[32];
  
  sha256.reset();
  sha256.update(data, strlen(data));
  sha256.finalize(value, sizeof(value));
  
  return toHex(hex, value, 32);
}

char *toHex(char *dest, uint8_t *src, int len) {
  char *d = dest;
  while( len-- ) {
    Serial.println((unsigned char)*src);
    sprintf(d, "%02x", (unsigned char)*src++);
    d += 2;
  }
  return dest;
}
*/
