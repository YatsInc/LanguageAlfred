# Language Alfred
**Voice assistant** that allows you to switch input languages by voice commands.

Currently supported for *switching*:
- English
- Ukrainian
- Russian

Currently supported for *recognizing*:
- English

Those values are hardcoded but there will be more languages supported dynamically in the future with offline voice recognition as well.

## How to build & use it

To be able to use it you have to create [Azure speech-to-text](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/speech-to-text) instance in Azure Portal and paste credentials in User Sercrets to match format provided below.

```
{
  "SubsribtionKey": "value",
  "Region": "value"
}
```

### **Future roadmap:**
- Background mode
- UI to be able to set keywords and operate languages
- Support of different voice recognition APIs
- Offline usage
- Support of Linux & MacOS
