#include <SHA256.h>

SHA256 sha256;


void setup() {
  
  randomSeed(analogRead(0));
  
  Serial.begin(9600);
  Serial.println();
  Serial.println("Started");

  char PK[66];
  privateKey(PK);
  
  Serial.println("Private Key:");
  Serial.println(PK);
  Serial.println();
  //PK = "1184CD2CDD640CA42CFC3A091C51D549B2F016D454B2774019C2B2D2E08529FD";

}

void loop() {

}

void privateKey(char *dest) {
  
  randomSeed(analogRead(0));
  uint8_t len = 33;
  uint8_t data[len];
  data[0] = 128; // 0x80;
  for(int r = 1; r < len; r++) {
    data[r] = (uint8_t)random(256);
  }
  
  uint8_t hexData[] = { 0x80, 0x11, 0x84, 0xCD, 0x2C, 0xDD, 0x64, 0x0C, 0xA4, 0x2C, 0xFC, 0x3A, 0x09, 0x1C, 0x51, 0xD5, 0x49, 0xB2, 0xF0, 0x16, 0xD4, 0x54, 0xB2, 0x77, 0x40, 0x19, 0xC2, 0xB2, 0xD2, 0xE0, 0x85, 0x29, 0xFD};
  
  Serial.println("Data:");
  char hexedData[(sizeof(hexData) * 2)];
  toHex(hexedData, hexData, sizeof(hexData));
  Serial.println(hexedData);
  Serial.println("801184CD2CDD640CA42CFC3A091C51D549B2F016D454B2774019C2B2D2E08529FD");
  Serial.println();

  
  uint8_t hashA[32];
  sha256.reset();
  sha256.update(hexData, sizeof(hexData));
  sha256.finalize(hashA, sizeof(hashA));
  
  char hexedHashA[(sizeof(hashA) * 2)];
  toHex(hexedHashA, hashA, sizeof(hashA));
  Serial.println(hexedHashA);
  Serial.println("FCFE4C3D371D7CC91C44294C520E2C2D51E74261448F945867ADB6BCFADDE839");
  Serial.println();
  
  uint8_t hashB[32];
  sha256.reset();
  sha256.update(hashA, sizeof(hashA));
  sha256.finalize(hashB, sizeof(hashB));
  
  char hexedHashB[(sizeof(hashB) * 2)];
  toHex(hexedHashB, hashB, sizeof(hashB));
  Serial.println(hexedHashB);
  Serial.println("206EC97EF3324B0149F7C18F46BC997778F0BCEDA46ED1B748574D5C83EA3F56");
  Serial.println();

  uint8_t checksum[4];
  memcpy(checksum, hashB, 4);
  
  char hexedChecksum[(sizeof(checksum) * 2)];
  toHex(hexedChecksum, checksum, sizeof(checksum));
  Serial.println(hexedChecksum);

  uint8_t WIF[37];
  for (uint8_t a = 0; a < sizeof(hexData); a++) {
    WIF[a] = hexData[a];
  }
  for (uint8_t b = 0; b < 4; b++) {
    WIF[sizeof(hexData) + b] = hashB[b];
  }
  
  char hexedWIF[(sizeof(WIF) * 2)];
  toHex(hexedWIF, WIF, sizeof(WIF));
  Serial.println(hexedWIF);
  Serial.println("801184CD2CDD640CA42CFC3A091C51D549B2F016D454B2774019C2B2D2E08529FD206EC97E");
  Serial.println();
}

void toHex(char *dest, uint8_t *src, int len) {
  uint8_t *d = dest;
  while( len-- ) {
    sprintf(d, "%02X", (unsigned char)*src++);
    d += 2;
  }
}
