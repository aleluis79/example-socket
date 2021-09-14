import { Component, OnInit } from '@angular/core';
import { webSocket, WebSocketSubject } from "rxjs/webSocket";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  subject: WebSocketSubject<unknown> | null = null;

  title = 'web';

  mensaje = '';

  ngOnInit(): void {

    this.subject = webSocket('ws://localhost:9000');

    this.subject.subscribe(
      msg => {
        console.log('message received: ' + (msg as any).mensaje)
      }, // Called whenever there is a message from the server.
      err => {
        console.log(err)
      }, // Called if at any point WebSocket API signals some kind of error.
      () => {
        console.log('complete')
      }// Called when connection is closed (for whatever reason).
    );
    // Note that at least one consumer has to subscribe to the created subject - otherwise "nexted" values will be just buffered and not sent,
    // since no connection was established!

    //this.subject.next({message: 'some message'});
    // This will send a message to the server once a connection is made. Remember value is serialized with JSON.stringify by default!

    //this.subject.complete(); // Closes the connection.

    //this.subject.error({code: 4000, reason: 'I think our app just broke!'});
    // Also closes the connection, but let's the server know that this closing is caused by some error.

  }


  enviar() {
    if (this.subject) {
      this.subject.next({message: this.mensaje});
    }
  }

  desconectar() {
    if (this.subject) {
      this.subject.complete();
    }
  }

}
