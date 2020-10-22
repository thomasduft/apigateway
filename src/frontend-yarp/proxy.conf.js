const PROXY_CONFIG = [
  {
    context: [
      "/"
    ],
    target: "https://localhost:5010",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
