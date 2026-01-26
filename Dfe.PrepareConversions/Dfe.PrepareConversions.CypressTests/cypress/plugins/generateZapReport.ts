const ZapClient = require('zaproxy');

interface ZapOptions {
    apiKey: string | undefined;
    proxy: {
        host: string | undefined;
        port: string | undefined;
    };
}

interface ZapRecordsToScanResponse {
    recordsToScan: string;
}

export async function generateZapReport(): Promise<void> {
    console.log('Generating ZAP report');

    const zapOptions: ZapOptions = {
        apiKey: process.env.ZAP_API_KEY,
        proxy: {
            host: process.env.ZAP_ADDRESS,
            port: process.env.ZAP_PORT,
        },
    };

    const zaproxy = new ZapClient(zapOptions);

    // Wait for passive scanner to finish scanning before generating report
    let recordsRemaining = 100;
    while (recordsRemaining !== 0) {
        await zaproxy.pscan
            .recordsToScan()
            .then((resp: ZapRecordsToScanResponse) => {
                try {
                    recordsRemaining = parseInt(resp.recordsToScan, 10);
                } catch (err) {
                    if (err instanceof Error) {
                        console.log(`Error converting result: ${err.message}`);
                    } else {
                        console.log('Unknown error during results conversion');
                    }
                    recordsRemaining = 0;
                }
            })
            .catch((err: unknown) => {
                console.log(`Error from the ZAP Passive Scan API: ${err}`);
                recordsRemaining = 0;
            });
    }

    await zaproxy.reports
        .generate({
            title: 'Report',
            template: 'traditional-html',
            reportfilename: 'ZAP-Report.html',
            reportdir: '/zap/wrk',
        })
        .then((resp: unknown) => {
            console.log(`${JSON.stringify(resp)}`);
        })
        .catch((err: unknown) => {
            console.log(`Error from ZAP Report API: ${err}`);
        });
}
