#include "Tools.h"

void Int32ToChar(char* des, INT32 src) {
    memcpy(des, &src, sizeof(src));
}