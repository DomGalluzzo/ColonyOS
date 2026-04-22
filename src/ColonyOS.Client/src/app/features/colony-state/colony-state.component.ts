import { Component, Input } from '@angular/core';
import { ColonyState } from '../../shared/models/colony-state.model';

@Component({
  selector: 'app-colony-state',
  templateUrl: './colony-state.component.html',
  styleUrls: ['./colony-state.component.scss']
})
export class ColonyStateComponent {
  @Input() colonyState: ColonyState;
}
