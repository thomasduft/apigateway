const PROXY_CONFIG = [
  {
    context: [
      "/"
    ],
    target: "https://localhost:5000",
    secure: false
  }
]

module.exports = PROXY_CONFIG;