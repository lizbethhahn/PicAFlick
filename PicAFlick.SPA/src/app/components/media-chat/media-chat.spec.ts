import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaChat } from './media-chat';

describe('MediaChat', () => {
  let component: MediaChat;
  let fixture: ComponentFixture<MediaChat>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MediaChat]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MediaChat);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
