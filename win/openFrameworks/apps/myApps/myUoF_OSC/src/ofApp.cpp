#include "ofApp.h"

#define PORT 8080
//--------------------------------------------------------------
void ofApp::setup(){
	ofBackground(0);
    ofSetFrameRate(60);

	font.loadFont("04B.TTF", 72);

	img.loadImage("lenna.jpg");

	screen.setup("127.0.0.1",PORT);
	screen.setOptions(512,512,50);
	screen.start();

	x = 0;
	y = 0;
}

//--------------------------------------------------------------
void ofApp::update(){
	
}

//--------------------------------------------------------------
void ofApp::draw(){

	static int num = 0;
	ofSetColor(255, 255, 255);
	font.drawString(ofToString(ofGetFrameRate(),2),300,200);
    font.drawString(ofToString(num++), 300, 300);

	img.draw(x,y);

	screen.get();
}

//--------------------------------------------------------------
void ofApp::keyPressed(int key){

}

//--------------------------------------------------------------
void ofApp::keyReleased(int key){

}

//--------------------------------------------------------------
void ofApp::mouseMoved(int x, int y ){

}

//--------------------------------------------------------------
void ofApp::mouseDragged(int x, int y, int button){
	ofApp::x = x;
	ofApp::y = y;
}

//--------------------------------------------------------------
void ofApp::mousePressed(int x, int y, int button){

}

//--------------------------------------------------------------
void ofApp::mouseReleased(int x, int y, int button){

}

//--------------------------------------------------------------
void ofApp::windowResized(int w, int h){

}

//--------------------------------------------------------------
void ofApp::gotMessage(ofMessage msg){

}

//--------------------------------------------------------------
void ofApp::dragEvent(ofDragInfo dragInfo){ 

}

void ofApp::exit(){
	screen.stop();
}