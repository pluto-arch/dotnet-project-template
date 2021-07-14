import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from '@angular/forms';

@Component({
  selector: 'app-login-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.less']
})
export class AccountComponent implements OnInit {
  validateForm!: FormGroup;
  showLoading = false;
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
    this.showLoading = !this.showLoading;
    setTimeout(() => {
      console.log(this.validateForm.value)
      this.showLoading = !this.showLoading;
    }, 5000);
  }

}