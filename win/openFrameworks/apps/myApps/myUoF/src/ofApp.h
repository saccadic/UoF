#pragma once

#include "ofMain.h"

#include "ofxOpenCv.h"
#include "ofxCv.h"

#include "ofxLibwebsockets.h"
//#include "ofxMat-to-buffer.h"

#include "ofxTurboJpeg.h"

using namespace cv;
using namespace ofxCv;
using namespace ofxLibwebsockets;

#define NUM_MESSAGES 30 // how many past messages we want to keep

class ofApp : public ofBaseApp{

	public:
		void setup();
		void update();
		void draw();

		void keyPressed(int key);
		void keyReleased(int key);
		void mouseMoved(int x, int y );
		void mouseDragged(int x, int y, int button);
		void mousePressed(int x, int y, int button);
		void mouseReleased(int x, int y, int button);
		void windowResized(int w, int h);
		void dragEvent(ofDragInfo dragInfo);
		void gotMessage(ofMessage msg);
		
		// opencv methods
		Mat buffer;
		ofImage img;

		//ofxMat-to-buffer
		//ofxMatToBuffer mtb;
		unsigned char *output_buff;
		int output_size;

		ofxTurboJpeg turbo;
					 unsigned long size;
			ofImage carrentimg;
			unsigned char * compressed;
			ofTrueTypeFont font;
		Connection client;
		bool req;
		int c;
		Mat tmp;

		// websocket methods
		ofxLibwebsockets::Server server;
        bool bSetup;

		void run_server(int port);
		void send(Connection conn,string msg);
		void send(Connection conn,ofBuffer data);
		void send(Connection conn,unsigned char* data, int size);

        void onConnect( ofxLibwebsockets::Event& args );
        void onOpen( ofxLibwebsockets::Event& args );
        void onClose( ofxLibwebsockets::Event& args );
        void onIdle( ofxLibwebsockets::Event& args );
        void onMessage( ofxLibwebsockets::Event& args );
		void onBroadcast( ofxLibwebsockets::Event& args );
};
