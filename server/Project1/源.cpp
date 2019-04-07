#include<stdio.h>
int abs(int x) {
    return x < 0 ? -x : x;
}

//#define abs(x) (x<0?-x:x)
void GetMax(int dp[][105][3], int a[],int s,int e) {
    int cur = dp[s][e][0];

    int value = dp[s + 1][e][0] + abs(a[s] - dp[s + 1][e][1]);
    if (value >= cur) {
        cur = value;
        dp[s][e][0] = cur;
        dp[s][e][1] = a[s];
        dp[s][e][2] = dp[s + 1][e][2];
    }

    value = dp[s + 1][e][0] + abs(a[s] - dp[s + 1][e][2]);
    if (value >= cur) {
        cur = value;
        dp[s][e][0] = cur;
        dp[s][e][1] = a[s];
        dp[s][e][2] = dp[s + 1][e][1];
    }

    value = dp[s][e - 1][0] + abs(a[e] - dp[s][e - 1][1]);
    if (value >= cur) {
        cur = value;
        dp[s][e][0] = cur;
        dp[s][e][1] = a[e];
        dp[s][e][2] = dp[s][e - 1][2];
    }

    value = dp[s][e - 1][0] + abs(a[e] - dp[s][e - 1][2]);
    if (value >= cur) {
        cur = value;
        dp[s][e][0] = cur;
        dp[s][e][1] = a[e];
        dp[s][e][2] = dp[s][e - 1][1];
    }
}
int main() {
    int n;
    while (~scanf("%d", &n)) {
        int a[105];
        for (int i = 1; i <= n; i++) {
            scanf("%d", &a[i]);
        }
        int dp[105][105][3] = { 0 };
        for (int l = 0; l < n; l++) {
            for (int i = 1; i <= n; i++) {
                if (l == 0) {
                    dp[i][i][0] = 0;
                    dp[i][i][1] = dp[i][i][2] = a[i];
                }
                else if (i + l > n) {
                    break;
                }
                else {
                    GetMax(dp, a, i, i + l);
                }
            }
        }
        //printf("%d %d %d %d\n", dp[1][2][0], dp[1][3][0], dp[1][3][1], dp[1][3][2]);
        printf("%d \n", dp[1][n][0]);
    }
    return 0;
}

