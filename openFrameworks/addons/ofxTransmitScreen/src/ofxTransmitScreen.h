#pragma once

#include "ofMain.h"
#include "ofxOsc.h"

#include "ofxTurboJpeg.h"

class ofxTransmitScreen : public ofThread{
public:
	ofxTransmitScreen(){
		ready     = false;
		sendReady = true;
	}

    void start()
    {
        startThread(true,false);
    }

    void stop()
    {
        stopThread();
    }

	void threadedFunction();

	void setup(string host,int port);

	void setOptions(int quality);
	void setOptions(int width,int heigh,int quality);

	void get();
	void send();

private:
	//Options
	bool ready;
	int width;
	int height;
	int quality;
	bool sendReady;

	//OSC options
	ofxOscSender sender;
	ofxOscMessage message;

	//Image Buffer
	ofImage ScreenBuffer;
	ofImage tmpBuffer;

	//TurboJpg options
	ofxTurboJpeg turbo;
	unsigned char * compressed;
	unsigned long size;
};