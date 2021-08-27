#include <SHA256.h>

SHA256 sha256;


void setup() {
  
  randomSeed(analogRead(0));
  
  Serial.begin(9600);
  Serial.println();
  Serial.println("Started");

  char PK[52];
  privateKey(PK, false);
  
  Serial.println("Wallet import format:");
  Serial.println(PK);
  Serial.println();
  
  x = 0x79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798
  y = 0x483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8
  p = 0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F
  assert((y*y - x*x*x - 7) % p == 0)
}

void loop() {

}

void privateKey(char *dest, bool debug) {
  
  randomSeed(analogRead(0));
  uint8_t hexData[33];
  hexData[0] = 128; // 0x80;
  for (int r = 1; r < sizeof(hexData); r++) {
    hexData[r] = (uint8_t) random(256);
  }

  if (debug) {
    uint8_t hexKnown[] = { 0x80, 0x11, 0x84, 0xCD, 0x2C, 0xDD, 0x64, 0x0C, 0xA4, 0x2C, 0xFC, 0x3A, 0x09, 0x1C, 0x51, 0xD5, 0x49, 0xB2, 0xF0, 0x16, 0xD4, 0x54, 0xB2, 0x77, 0x40, 0x19, 0xC2, 0xB2, 0xD2, 0xE0, 0x85, 0x29, 0xFD, '\0' };
    memcpy(hexData, hexKnown, sizeof(hexData) + 1);
    
    char hexedData[(sizeof(hexData) * 2)];
    toHex(hexedData, hexData, sizeof(hexData));
    Serial.println(hexedData);
    Serial.println("801184CD2CDD640CA42CFC3A091C51D549B2F016D454B2774019C2B2D2E08529FD");
    Serial.println();
  }
    
  uint8_t hashA[32];
  sha256.reset();
  sha256.update(hexData, sizeof(hexData));
  sha256.finalize(hashA, sizeof(hashA));

  if (debug) {    
    char hexedHashA[(sizeof(hashA) * 2)];
    toHex(hexedHashA, hashA, sizeof(hashA));
    Serial.println(hexedHashA);
    Serial.println("FCFE4C3D371D7CC91C44294C520E2C2D51E74261448F945867ADB6BCFADDE839");
    Serial.println();
  }
  
  uint8_t hashB[32];
  sha256.reset();
  sha256.update(hashA, sizeof(hashA));
  sha256.finalize(hashB, sizeof(hashB));

  if (debug) {    
    char hexedHashB[(sizeof(hashB) * 2)];
    toHex(hexedHashB, hashB, sizeof(hashB));
    Serial.println(hexedHashB);
    Serial.println("206EC97EF3324B0149F7C18F46BC997778F0BCEDA46ED1B748574D5C83EA3F56");
    Serial.println();
  }

  uint8_t checksum[4];
  memcpy(checksum, hashB, 4);
  
  if (debug) {    
    char hexedChecksum[(sizeof(checksum) * 2)];
    toHex(hexedChecksum, checksum, sizeof(checksum));
    
    Serial.println(hexedChecksum);
    Serial.println("206EC97E");
    Serial.println();
  }

  uint8_t keyedChecksum[37];
  for (uint8_t a = 0; a < sizeof(hexData); a++) {
    keyedChecksum[a] = hexData[a];
  }
  for (uint8_t b = 0; b < 4; b++) {
    keyedChecksum[sizeof(hexData) + b] = hashB[b];
  }
  
  if (debug) {    
    char hexedKeyedChecksum[(sizeof(keyedChecksum) * 2)];
    toHex(hexedKeyedChecksum, keyedChecksum, sizeof(keyedChecksum));
    Serial.println(hexedKeyedChecksum);
    Serial.println("801184CD2CDD640CA42CFC3A091C51D549B2F016D454B2774019C2B2D2E08529FD206EC97E");
    Serial.println();
  }
  
  static unsigned char alphabet[] = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
  
  uint8_t * input = keyedChecksum;
  int inLen = sizeof(keyedChecksum);
  
  char outputKey[51];
  char * output = outputKey;
  int outLen = sizeof(outputKey);
  
  memset(output, 0, outLen);
  for(int i = 0; i < inLen; i++)
  {
    unsigned int c = input[i] & (0xff);
    for(int j = outLen - 1; j >= 0; j--)
    {
      int tmp = output[j] * 256 + c;
      c = tmp / 58;
      output[j] = tmp % 58;
    }
  }
  output[inLen] = '\0';
  
  for(int j = 0; j < sizeof(outputKey); j++)
  {
    output[j] = alphabet[output[j]];
  }
  output[outLen] = '\0';

  if (debug) {
    Serial.println(outputKey);
    Serial.println("5Hx15HFGyep2CfPxsJKe2fXJsCVn5DEiyoeGGF6JZjGbTRnqfiD");
    Serial.println();
  }

  memcpy(dest, outputKey, sizeof(outputKey));
  dest[51] = '\0';
}

void toHex(char *dest, uint8_t *src, int len) {
  uint8_t *d = dest;
  while( len-- ) {
    sprintf(d, "%02X", (unsigned char)*src++);
    d += 2;
  }
}
