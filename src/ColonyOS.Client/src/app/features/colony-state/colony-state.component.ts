import { Component, Input } from '@angular/core';
import { ColonyState } from '../../shared/models/colony-state.model';
import { ColonyResource } from '../../shared/models/colony-resource.model';

@Component({
  selector: 'app-colony-state',
  templateUrl: './colony-state.component.html',
  styleUrls: ['./colony-state.component.scss']
})
export class ColonyStateComponent {
  @Input() colonyState: ColonyState;

  public isBelowMin(resource: ColonyResource): boolean {
    return resource.minThreshold !== null && resource.percentage < resource.minThreshold;
  }

  public isAboveMax(resource: ColonyResource): boolean {
    return resource.maxThreshold !== null && resource.percentage > resource.maxThreshold;
  }

  getResourceStatus(resource: ColonyResource): 'critical-low' | 'warning-low' | 'normal' | 'warning-high' | 'critical-high' {
    const thresholdBuffer = 10;

    if (resource.minThreshold !== null) {
      if (resource.percentage < resource.minThreshold) {
        return 'critical-low';
      }

      if (resource.percentage <= resource.minThreshold + thresholdBuffer) {
        return 'warning-low';
      }
    }

    if (resource.maxThreshold !== null) {
      if (resource.percentage > resource.maxThreshold) {
        return 'critical-high';
      }

      if (resource.percentage >= resource.maxThreshold - thresholdBuffer) {
        return 'warning-high';
      }
    }

    return 'normal';
  }
}
