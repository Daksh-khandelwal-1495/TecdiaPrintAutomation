# 🖨️ Syteline Local Print API

This project enables a seamless integration between **Infor Syteline (CloudSuite Industrial)** and a **local printer** using a lightweight HTTP API exposed over the internet via **ngrok**.

It allows remote instances of Syteline to trigger printing on a local or network-connected printer, ideal for use cases such as printing **shipping labels**, **product stickers**, or **batch receipts**.

---

## 🔧 Features

* Exposes a `/Print` endpoint to receive printing requests
* Accepts simple JSON payloads
* Sends content directly to the local default printer
* Uses `ngrok` to expose local API securely to the internet
* Plug-and-play integration with Syteline custom form scripting

---

## 📝 Requirements

| Tool              | Version      | Purpose                        |
| ----------------- | ------------ | ------------------------------ |
| .NET SDK          | 6.0 or later | To build and run the API       |
| ngrok CLI         | Any          | Expose local service via HTTPS |
| Windows OS        | 10+          | Required for printer access    |
| Printer Installed | Any          | Should be set as default       |

## 🔧 Requirements

| Tool             | Version      | Purpose                        |
|------------------|--------------|--------------------------------|
| [.NET SDK](https://dotnet.microsoft.com/en-us/download) | 6.0+ | To build and run the API |
| [ngrok](https://ngrok.com/download) | Any (CLI version) | Tunnel public HTTPS to localhost |
| Windows OS | Any modern version | To access local printers |
---

## 📂 Project Structure

```
/PrintAPI
├── Controllers/
│   └── PrintController.cs      # POST /Print logic
├── Program.cs                  # App startup logic
├── PrinterHelper.cs            # Raw printing logic
├── appsettings.json            # Configs (optional)
├── README.md                   # This file
```

---

## ▶️ Run the API Locally

### 1. Clone and Build

```bash
git clone https://github.com/yourusername/PrintAPI.git
cd PrintAPI
dotnet run --urls "http://localhost:5131"
```

> This starts the local API server at: `http://localhost:5131/Print`

---

## 🌐 Expose the API using ngrok

### 2. Install ngrok

Download from: [https://ngrok.com/download](https://ngrok.com/download) and add it to your system path.

### 3. Authenticate ngrok (once)

```bash
ngrok config add-authtoken <your-auth-token>
```

### 4. Start Tunnel

```bash
ngrok http 5131
```

You’ll receive a URL like:

```
https://77b6ce266157.ngrok-free.app
```

### 5. Test the Endpoint

Send a POST request to:

```
https://77b6ce266157.ngrok-free.app/Print
```

With a JSON payload:

```json
{
  "text": "Print this label text"
}
```

You should see the content printed on your default printer.

---

## 🚀 Integration with Syteline

In your Syteline Form Script:

```vb
url = "https://77b6ce266157.ngrok-free.app/Print"
' Send HTTP POST from script with JSON payload to trigger printing
```

> Update the URL every time ngrok is restarted (unless using a paid static subdomain).

---

## 🏢 Production Deployment Suggestions

| Option          | Description                                |
| --------------- | ------------------------------------------ |
| ngrok Pro       | Use static subdomain so URL doesn’t change |
| Windows Service | Convert API to run in background on boot   |
| Packaging       | Bundle .NET app + ngrok into installer     |
| Logging         | Add print logs to file or DB               |
| Security        | Add API key or IP filtering to endpoint    |

---

## 🚨 Known Limitations

* **Free ngrok URL changes on every restart**
* **System must remain on** to keep the tunnel active
* **Not suitable for high-volume enterprise scale without enhancements**

---

## 🎯 Future Improvements

* UI dashboard for logs & config
* Multi-printer support
* Print preview & validation layer
* Printer queue retry logic

---

## 💬 Contact

Developed by **Daksh Khandelwal and Devesh Chahar**

---

> "Print locally, from anywhere."
