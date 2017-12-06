#include <stdio.h>
#include <stdlib.h>
#include <tchar.h>

#pragma pack(1)
typedef struct _TestStruct {
  int m_IntArray[10];
  int m_Int;
} TestStruct;

void printTestStruct(const TestStruct iTestStruct) {
	int i;

	printf("{");
	printf("{");
	for (i = 0; i<10; i++) {
		printf("%d%s", iTestStruct.m_IntArray[i], (i==9?"":", "));
	}
	printf("}, ");
	printf("%d}", iTestStruct.m_Int);
}

void writeStructsToFile(TestStruct *const pTestStruct, const unsigned n, const char *fileName) {
	FILE *fp;
	unsigned j;

	// Perform Struct Assignment...
	for(j=0; j<n; ++j)	{
		int i;
		int s=0;

		for (i = 0; i<10; i++) {
			s += i*(j+1);
			pTestStruct[j] . m_IntArray[i] = i*(j+1);
		}
		pTestStruct[j] . m_Int = s;
	}

	fp = fopen(fileName, "wb");
	if(fp == NULL) {
		fprintf(stderr, "Failed to open file : %s", fileName);
	}

	// Write Structs to File...
	fwrite((void *)(pTestStruct), sizeof(TestStruct), n, fp);
	for(j=0; j<n; ++j)	{
		printf("\npTestStruct[%d] : ", j);
		printTestStruct(pTestStruct[j]);
	}
	fclose(fp);

	printf("\n%s\n", "Structs written to file...");
}

void readStructsFromFile(TestStruct *const pTestStruct, const unsigned int n, const char *fileName) {
	FILE *fp;
	unsigned j;

	fp = fopen(fileName, "rb");
	if(fp == NULL) {
		fprintf(stderr, "Failed to open file : %s", fileName);
	}

	// Read Struct from File...
	fread((void *)(pTestStruct), sizeof(TestStruct), n, fp);
	for(j=0; j<n; ++j)	{
		printf("\npTestStruct[%d] : ", j);
		printTestStruct(pTestStruct[j]);
	}
	fclose(fp);

	printf("\n%s\n", "Structs read from file...");
}

int _tmain(int argc, _TCHAR* argv[])
{
	TestStruct pTestStructWrite[5];
	TestStruct pTestStructRead[5];
	const char fileName[] = "C:\\Users\\SRY8\\Desktop\\structWrite.bin";

	printf("sizeof(TestStruct) : %d\n", sizeof(TestStruct));

	writeStructsToFile(pTestStructWrite, 5, fileName);
	readStructsFromFile(pTestStructRead, 5, fileName);

	return 0;
}