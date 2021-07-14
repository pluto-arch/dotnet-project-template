import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from '@angular/forms';

@Component({
  selector: "app-login-mobile",
  templateUrl: './mobile.component.html',
  styleUrls: ['./mobile.component.less']
})
export class MobileComponent implements OnInit {
  validateForm!: FormGroup;
  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.validateForm = this.fb.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]],
      remember: [true]
    });
  }

  submitForm(): void {
    for (const i in this.validateForm.controls) {
      this.validateForm.controls[i].markAsDirty();
      this.validateForm.controls[i].updateValueAndValidity();
    }
  }
}