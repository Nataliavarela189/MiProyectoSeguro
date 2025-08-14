import { Component, OnInit } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { Person } from '../../models/person.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss']
})
export class PersonListComponent implements OnInit {
  persons: Person[] = []; // Inicializamos como array vacío

  constructor(
    private personService: PersonService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadPersons();
  }

  loadPersons(): void {
    this.personService.getAll().subscribe({
      next: (data: Person[]) => this.persons = data,
      error: (err: any) => console.error('Error al obtener personas', err)
    });
  }

  editPerson(id: number): void {
    this.router.navigate(['/persons/edit', id]);
  }

  deletePerson(id: number): void {
    if (confirm('¿Estás seguro que querés eliminar esta persona?')) {
      this.personService.delete(id).subscribe(() => {
        this.loadPersons(); // Recarga lista luego de eliminar
      }, error => {
        console.error('Error al eliminar persona', error);
      });
    }
  }
}



