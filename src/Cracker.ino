#include <Crypto.h>
#include <SHA256.h>

SHA256 sha256;


void setup() {
  
  randomSeed(analogRead(0));
  
  Serial.begin(9600);
  Serial.println("Started");


  Serial.println("Private Key:");
  char PK[66];
  privateKey(PK);
  Serial.println(PK);

  PK = "1184CD2CDD640CA42CFC3A091C51D549B2F016D454B2774019C2B2D2E08529FD";

}

void loop() {

}

void privateKey(char *dest) {
  
  randomSeed(analogRead(0));
  
  uint8_t key[32];
  for(int r = 0; r < 32; r++) {
    key[r] = (uint8_t)random(256);
  }
  
  char hexed[32];
  toHex(hexed, key, 32);
  //Serial.println("hexed:");
  //Serial.println(hexed);
  
  char uppered[64];
  toUpperArray(uppered, hexed);
  //Serial.println("uppered:");
  //Serial.println(uppered);
  
  char * prefixed = dest;
  prefixed[0] = '8';
  prefixed[1] = '0';
  
  int len = strlen(uppered);
  for(int c = 0; c < len; c++) {
    prefixed[c + 2] = uppered[c];
  }
  prefixed[66] = '\0';
  //Serial.println("prefixed:");
  //Serial.println(prefixed);
    
  return dest;
}

void toHex(char *dest, uint8_t *src, int len) {
  uint8_t *d = dest;
  while( len-- ) {
    sprintf(d, "%02x", (unsigned char)*src++);
    d += 2;
  }
}

void toUpperArray(char *dest, char *value) {
  int len = strlen(value);
  for(int c = 0; c < len; c++) {
    value[c] = toupper(value[c]);
  }
  memcpy(dest, value, 64);
  dest[64] = '\0';
}

char toUpper(char c) {
  return toupper(c);
}
