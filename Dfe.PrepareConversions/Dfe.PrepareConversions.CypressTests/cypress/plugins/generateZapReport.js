const ZapClient = require('zaproxy')
const fs = require('fs')

module.exports = {
    generateZapReport: async () => {
      const zapOptions = {
        apiKey: process.env.zapApiKey || '',
        proxy: process.env.zapUrl || 'http://localhost:8080'
      }
      const zaproxy = new ZapClient(zapOptions)
      try {
        const res = await zaproxy.core.numberOfAlerts()
        // TODO Investigate HTML report currently causing ZAP to abort request
        await zaproxy.core.mdreport()
        .then(
          resp => {
            if(!fs.existsSync('./reports')) {
              fs.mkdirSync('./reports')
            }
            fs.writeFileSync('./reports/ZAP-Report.md', resp)
          },
          err => {
            console.log(`Error during report generation: ${err}`)
          }
        )
      } catch (err) {
        console.log(`Error contacting the ZAP API: ${err}`)
      }
    }
  }