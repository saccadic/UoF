#include "ofApp.h"

#define PORT 8080

//--------------------------------------------------------------
void ofApp::setup(){
	run_server(PORT);

    ofBackground(0);
    ofSetFrameRate(60);

	img.loadImage("lenna.jpg");
	buffer = toCv(img);
	
	//Mat red_img(cv::Size(500, 500), CV_8UC3, cv::Scalar(0,0,255));
	//buffer = red_img.clone();
	toOf(buffer,carrentimg);
	//mtb.jpg_to_memory(buffer,100);              //Pixcel to memory
	//output_size = mtb.get_buffer(&output_buff); //Get Binary
	carrentimg = img;
	req = false;
	c = 0;

	//ofSaveFrame();
	carrentimg.allocate(ofGetWidth(), ofGetHeight(), OF_IMAGE_COLOR);
	//server.sleep(1);
	font.loadFont("04B.TTF", 72);
}

//--------------------------------------------------------------
void ofApp::update(){



	/*
	

	if(req){
		switch (c){
		case 0:
			c = 1;
			flip(buffer,tmp,0);
			break;
		case 1:
			c = 2;
			flip(buffer,tmp,1);
			break;
		case 2:
			c = 0;
			flip(buffer,tmp,-1);
			break;
		default:
			break;
		}

		toOf(tmp,carrentimg);

		carrentimg.resize(128,128);

		unsigned char * compressed = turbo.compress(carrentimg,50,&size);
		//unsigned char compressed[10000] = {0};
		//size = 9999;
		
		client.sendBinary(compressed,size);
	}
	*/
	if(req){
		static int num = 0;
		if(num++ > 1){
			num = 0;

			switch (c){
			case 0:
				c = 1;
				flip(buffer,tmp,0);
				break;
			case 1:
				c = 2;
				flip(buffer,tmp,1);
				break;
			case 2:
				c = 0;
				flip(buffer,tmp,-1);
				break;
			default:
				break;
			}

			carrentimg.grabScreen(0, 0, ofGetWidth(), ofGetHeight());
			carrentimg.resize(50,50);
			compressed = turbo.compress(carrentimg,50,&size);
			client.sendBinary(compressed,size);
			client.update();
			//client.send(to_string(size));
			//unsigned char compressed[10000] = {0};
			//size = 9999;
			cout << "send" << endl;
		}
	}
}

//--------------------------------------------------------------
void ofApp::draw(){
	static int num = 0;
	
	drawMat(tmp,0,0);

	ofSetColor(255, 255, 255);
	
    font.drawString(ofToString(num++), 300, 300);
			
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
void ofApp::onConnect( ofxLibwebsockets::Event& args ){
    cout<< "on connected" + args.conn.getClientIP() + ", " + args.conn.getClientName() <<endl;
}

//--------------------------------------------------------------
void ofApp::onOpen( ofxLibwebsockets::Event& args ){
    cout<< "new connection open"<<endl;
    cout<< "New connection from " + args.conn.getClientIP() + ", " + args.conn.getClientName() << endl;
}

//--------------------------------------------------------------
void ofApp::onClose( ofxLibwebsockets::Event& args ){
    cout<< "on close" + args.conn.getClientName() <<endl;
}

//--------------------------------------------------------------
   void ofApp::onIdle( ofxLibwebsockets::Event& args ){
    //cout<< "on idle: " <<endl;
}

//--------------------------------------------------------------
void ofApp::onMessage( ofxLibwebsockets::Event& args ){
   cout<< "got message " << args.message <<endl;
    
    // trace out string messages or JSON messages!
    if ( !args.json.isNull() ){
        
    } else {
        
    }

	// Binary data
    if ( !args.isBinary ){
        
    } else {
        
    }

	if(args.message == "img"){
		
			switch (c){
			case 0:
				c = 1;
				flip(buffer,tmp,0);
				break;
			case 1:
				c = 2;
				flip(buffer,tmp,1);
				break;
			case 2:
				c = 0;
				flip(buffer,tmp,-1);
				break;
			default:
				break;
			}

			client = args.conn;
			//toOf(tmp,carrentimg);

			//args.conn.sendBinary(compressed,size);
	}

	if(args.message == "start"){
		req = true;
		client = args.conn;
		//args.conn.sendBinary(output_buff,output_size);
		//send(args.conn,output_buff,output_size);
		//send(args.conn,"out");
	}
	if(args.message == "end"){
		req = false;
	}

	if(args.message == "echo"){
		args.conn.send("echo");
	}
}

//--------------------------------------------------------------
void ofApp::onBroadcast( ofxLibwebsockets::Event& args ){
    cout<<"got broadcast "<<args.message<<endl;    
}

//--------------------------------------------------------------
void ofApp::run_server(int port){
	ofxLibwebsockets::ServerOptions options = ofxLibwebsockets::defaultServerOptions();
    options.port = port;
    bSetup = server.setup( options );

	if(bSetup){
		cout << "WebSocket server setup at "+ofToString( server.getPort() ) + ( server.usingSSL() ? " with SSL" : " without SSL") << endl;
	}else{
		cout << "WebSocket setup failed." << endl;
	}

    server.addListener(this);
}

void ofApp::send(Connection conn,string msg){
	conn.send(msg);
}

void ofApp::send(Connection conn,ofBuffer data){
	conn.sendBinary(data);
}

void ofApp::send(Connection conn,unsigned char* data, int size){
	conn.sendBinary(data,size);
}


