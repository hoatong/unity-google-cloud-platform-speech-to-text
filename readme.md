# Unity GCP Speecd To Text

This script requies a service access settings object for specifying the authorization parameters of the speech to text client. To create an access settings object, right-click on the Assets window and select `Create/GCP Speech To Text/Access Settings`. You will need to provide the GCP project ID associated to your project, the service account and the filename of the .p12 private key file you placed into the `Resources\GCPSpeech2Text` folder.
To create .p12 private file key, follow  [this blog post](https://alessandrotironigamedev.com/2019/04/20/implementing-chatbots-in-your-unity-project-with-dialogflow-v2/)
