#include "ofApp.h"

//--------------------------------------------------------------
void ofApp::setup(){
	ofBackground(1);
	ofSetFrameRate(60);

	camera.setDeviceID(0);
	camera.initGrabber(320, 240);
	inputOfImg.allocate(320, 240);

    screen.setup("127.0.0.1",8080);
    screen.setOptions(800,800,70);
    screen.start();

	//screen.get();
}

//--------------------------------------------------------------
void ofApp::update(){
	static int num = 0;

	camera.update();
	inputOfImg.setFromPixels(camera.getPixels(),320,240);
	//inputOfImg.invert();
	buffer = inputOfImg.getCvImage();
	//cv::medianBlur(buffer, buffer, 29);
	toOf(buffer,img);
	img.resize(ofGetWidth(), ofGetHeight());
	
	screen.get();
}

//--------------------------------------------------------------
void ofApp::draw(){
	
	img.draw(0,0);

	
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

//--------------------------------------------------------------
void ofApp::exit(){ 
	screen.stop();
}