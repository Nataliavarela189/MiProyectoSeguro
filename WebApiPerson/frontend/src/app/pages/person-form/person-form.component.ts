import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PersonService } from '../../services/person.service';
import { Person } from '../../models/person.model';

@Component({
  selector: 'app-person-form',
  templateUrl: './person-form.component.html',
  styleUrls: ['./person-form.component.scss']
})
export class PersonFormComponent implements OnInit {
  personForm!: FormGroup;
  personId?: number;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private personService: PersonService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.personId = Number(this.route.snapshot.paramMap.get('id'));
    this.isEditMode = !!this.personId;

    this.personForm = this.fb.group({
      name: ['', Validators.required],
      identification: ['', Validators.required],
      age: [null, [Validators.required, Validators.min(0)]],
      gender: ['', Validators.required],
      isActive: [true],
      drives: [false],
      wearsGlasses: [false],
      hasDiabetes: [false],
      otherDiseases: ['']
    });

    if (this.isEditMode) {
      this.personService.getById(this.personId!).subscribe(person => {
        this.personForm.patchValue(person);
      });
    }
  }

  onSubmit(): void {
    if (this.personForm.invalid) {
      this.personForm.markAllAsTouched();
      return;
    }

    if (this.isEditMode) {
      const personData: Person = { id: this.personId!, ...this.personForm.value };
      this.personService.update(this.personId!, personData).subscribe(() => {
        this.router.navigate(['/persons']);
      });
    } else {
      const personData: Person = this.personForm.value;
      this.personService.create(personData).subscribe(() => {
        this.router.navigate(['/persons']);
      });
    }
  }


  cancel(): void {
    this.router.navigate(['/persons']);
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.personForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }
}

